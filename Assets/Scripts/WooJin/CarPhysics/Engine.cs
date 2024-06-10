using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EEngineState
{
    Lock, Acc, On, Start
}

public class Engine : MonoBehaviour
{
    private static Engine _instance;
    public static Engine Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Engine>();
            return _instance;
        }
    }

    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }



    private const float RPM2RPS = Mathf.PI / 30f;
    private const float RPS2RPM = 30f / Mathf.PI;

    [Header("Inputs")]
    private Rigidbody carBody;
    private Air air;
    [SerializeField] private PlayerCarInput input;
    //[SerializeField] private Battery battery;
    [Range(0f, 1f)]
    [SerializeField] private float _throttlePosition;
    public float throttlePosition {get => _throttlePosition; set => _throttlePosition = value;}
    public float carSpeed; //[m/s]
    public float fanSpeed;
    private float airSpeed => Mathf.Abs(carSpeed * 0.1f + fanSpeed);
    public bool isAccel = false;

    [Header("Characteristics")]

    [SerializeField] private EEngineState state;
    public int State {get => (int)state; set => state = (EEngineState)value;}
    
    [SerializeField] private AnimationCurve TorqueCurve;

    private const float maxEngineRPM = 7000;
    private const float maxFanRPM = 3000;

    private const float minInitRPM = 800;
    private const float maxInitRPM = 1000;

    private const float minDesireTemp = 90f;
    private const float maxDesireTemp = 104f;
    private const float engineSpecificHeat = 161460f;

    private const float shaftInertia = 1320f;

    [Header("Outputs")]

    public AutomaticTransmission transmission;

    public float currentTorque; // [T]
    public float currentPower; // [J]
    public float currentEngineRPM; // [RPM]

    public float currentTemperature; // [°C]

    void Start()
    {
        air = Air.Instance;
        currentTemperature = air.Temperature;
        carBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isAccel) {
            if (state == EEngineState.Start) {
                currentEngineRPM = 1200f;
            }
            if (state == EEngineState.On && currentEngineRPM > 0) {
                _throttlePosition += (input.GasUserInput - 0.5f) * Time.deltaTime;
                _throttlePosition = Mathf.Clamp01(_throttlePosition);
            }
            if (state == EEngineState.Lock) {
                currentEngineRPM *= 0.99f;
            }
            ModifyEngineRPM(Time.deltaTime);
        } else {
            currentEngineRPM += (9500 - currentEngineRPM) * Time.fixedDeltaTime;
        }

        float h = 2f * Mathf.Pow(airSpeed * 0.005f, 0.8f) * Mathf.Pow(air.Humidity, 0.4f); // [W/(m^2 * K)]
        
        ModifyTemperature(h, Time.deltaTime);

        SendToTransmission();
    }

    private void FixedUpdate() {
        carSpeed = Vector3.Dot(transform.forward, carBody.velocity);

        EvaluateTorque(_throttlePosition, Time.fixedDeltaTime);

    }


    private void EvaluateTorque(float throttlePos, float dt) {
        currentEngineRPM += throttlePos * dt;
        currentTorque = TorqueCurve.Evaluate(currentEngineRPM * 0.001f) * 100 * throttlePos;
    }

    private void ModifyEngineRPM(float dt) {
        currentEngineRPM += currentTorque / shaftInertia * dt;
        if (state == EEngineState.On && currentEngineRPM > 0) {
            // Add rpm based on throttle amount
            currentEngineRPM += 0.4f * _throttlePosition * (maxEngineRPM - currentEngineRPM) * dt;
            // Loss of rpm due to friction and resistance
            currentEngineRPM -= (currentEngineRPM - 850) * 0.2f * dt;
        }
    }

    private void ModifyTemperature(float h, float dt) {
        float lastTemperature = currentTemperature;
        // Temperature rising due to engine rpm
        currentTemperature += 100f * Mathf.Pow(currentEngineRPM, 1.2f) / engineSpecificHeat * dt;
        // Radiator cooling engine
        currentTemperature -= h * (currentTemperature - air.Temperature) * dt;

        float temperatureDiff = (currentTemperature - lastTemperature) / dt;
        float estimateTemp = currentTemperature + temperatureDiff * 3f;
        if (estimateTemp > maxDesireTemp) fanSpeed = (estimateTemp - maxDesireTemp) * 0.3f;
        else fanSpeed *= 1 - Mathf.Exp(-dt * 10f);
    }

    void SendToTransmission() {
        transmission.inputRPM = currentEngineRPM;
    }


    void OnCollisionEnter(Collision other)
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Clear || GameManager.Instance.gameState == GameManager.GameState.Null) return;
        carBody.constraints = RigidbodyConstraints.FreezeAll;
        GameManagerTalker.instance.gameOver();
    }
}
