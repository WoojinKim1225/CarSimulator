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

    private const float maxEngineRPM = 7000;
    private const float maxFanRPM = 3000;

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
        if (state == EEngineState.Start) {
            currentEngineRPM = 1000f;
        }
        //throttlePosition = input.GasUserInput;


        SendToTransmission();
    }

    private void FixedUpdate() {
        carSpeed = Vector3.Dot(transform.forward, carBody.velocity);

        EvaluateTorque();

        ModifyEngineRPM(Time.fixedDeltaTime);

        float h = 2f * Mathf.Pow(Mathf.Abs(airSpeed * 0.0005f), 0.8f) * Mathf.Pow(Mathf.Clamp01(air.Humidity), 0.4f); // [W/(m^2 * K)]
        
        ModifyTemperature(h, Time.fixedDeltaTime);
        
    }


    private void EvaluateTorque() {
        currentTorque = TorqueCurve.Evaluate(currentEngineRPM * 0.01f) * 100;
    }

    private void ModifyEngineRPM(float dt) {
        currentEngineRPM += currentTorque / shaftInertia * dt;
        if (state == EEngineState.On && currentEngineRPM > 0) {
            // Add rpm based on throttle amount
            currentEngineRPM += 0.3f * _throttlePosition * (maxEngineRPM - currentEngineRPM) * dt;
            // Loss of rpm due to friction and resistance
            currentEngineRPM -= 0.1f * dt;
        }
    }

    private void ModifyTemperature(float h, float dt) {
        // Temperature rising due to engine rpm
        currentTemperature += 80f * Mathf.Pow(currentEngineRPM, 1.1f) / engineSpecificHeat * dt;
        // Radiator cooling engine
        currentTemperature -= h * (currentTemperature - air.Temperature) * dt;
    }

    void SendToTransmission() {
        //transmission.inputRPM = currentEngineRPM;
    }
}
