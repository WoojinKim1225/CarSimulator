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
    private const float RPM2RPS = Mathf.PI / 30f;
    private const float RPS2RPM = 30f / Mathf.PI;

    [Header("Inputs")]
    private Rigidbody carBody;
    private Air air;
    [SerializeField] private PlayerCarInput input;
    //[SerializeField] private Battery battery;
    [Range(0f, 1f)]
    [SerializeField] private float _throttlePosition;
    public float throttlePosition {set => _throttlePosition = value;}
    public float carSpeed;
    public float fanSpeed;
    public float airSpeed => carSpeed + fanSpeed;

    [Header("Characteristics")]

    [SerializeField] private EEngineState state;
    
    [SerializeField] private AnimationCurve TorqueCurve;

    [SerializeField] private float maxEngineRPM = 7000;
    [SerializeField] private float maxFanRPM = 3000;

    [SerializeField] private float minDesireTemp = 90f;
    [SerializeField] private float maxDesireTemp = 104f;
    //public float Temp;

    [Header("Outputs")]

    public AutomaticTransmission transmission;

    public float currentTorque; // [T]
    public float currentPower; // [J]
    public float currentEngineRPM; // [RPM]

    public float currentTemperature; // [°C]

    private float angularVelocityOutput => currentEngineRPM * RPM2RPS;
    // Start is called before the first frame update
    void Start()
    {
        air = Air.Instance;
        currentTemperature = air.Temperature;
        carBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EEngineState.Start) {
            currentEngineRPM = 1000f;
        }
        //throttlePosition = input.GasUserInput;


        SendToTransmission();
    }

    private void FixedUpdate() {
        carSpeed = Vector3.Dot(transform.forward, carBody.velocity);

        EvaluateTorque();

        ModifyEngineRPM();

        float h = 2f * Mathf.Pow(Mathf.Abs(airSpeed * 0.0005f), 0.8f) * Mathf.Pow(Mathf.Clamp01(air.Humidity), 0.4f); // [W/(m^2 * K)]

        
        ModifyTemperature(h);
        
    }


    private void EvaluateTorque() {
        currentTorque = TorqueCurve.Evaluate(currentEngineRPM * 0.01f) * 100;
    }

    private void ModifyEngineRPM() {
        if (state == EEngineState.On && currentEngineRPM > 0) {
            currentEngineRPM += 0.3f * _throttlePosition * (maxEngineRPM - currentEngineRPM) * Time.fixedDeltaTime;
            currentEngineRPM -= 0.1f * Time.fixedDeltaTime;
        }
    }

    private void ModifyTemperature(float h) {
        currentTemperature += 80f * Mathf.Pow(currentEngineRPM, 1.1f) / 161460f * Time.deltaTime;
        currentTemperature -= h * (currentTemperature - air.Temperature) * Time.deltaTime;
    }

    void SendToTransmission() {
        transmission.inputRPM = angularVelocityOutput;
    }
}
