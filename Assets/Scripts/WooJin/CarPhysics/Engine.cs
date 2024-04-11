using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEngineState
{
    Lock, Acc, On, Start
}

public class Engine : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private PlayerCarInput input;
    //[SerializeField] private Battery battery;
    [Range(0f, 1f)]
    [SerializeField] private float _throttlePosition;
    public float throttlePosition {set => _throttlePosition = value;}

    [Header("Characteristics")]

    [SerializeField] private EEngineState state;
    
    [SerializeField] private AnimationCurve PowerCurve;
    [SerializeField] private AnimationCurve TorqueCurve;
    [SerializeField] private AnimationCurve FuelConsumptionMap;

    [SerializeField] private float maxRPM = 7000;

    [Header("Outputs")]

    public AutomaticTransmission transmission;

    public float currentTorque; // [T]
    public float currentPower; // [J]
    public float currentAngularVelocity; // [rad/s]

    private float angularVelocityOutput => currentAngularVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EEngineState.Start) {
            currentAngularVelocity = 104.8f;
        }
        throttlePosition = input.GasUserInput;
        currentTorque = TorqueCurve.Evaluate(currentAngularVelocity * 0.01f) * 100f;
        currentPower = PowerCurve.Evaluate(currentAngularVelocity * 0.01f) * 10000f;

        SendOutputToTransmission();
    }

    void SendOutputToTransmission() {
        transmission.Input.angularVelocity = angularVelocityOutput;
        transmission.Input.torque = currentTorque;
        transmission.Input.power = currentPower;
    }
}
