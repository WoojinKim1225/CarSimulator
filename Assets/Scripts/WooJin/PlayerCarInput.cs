using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarInput : MonoBehaviour
{
    private CarControlActionAsset inputAction;
    private float steerUserInput;
    private float steerUserInputDamped;
    public float SteerUserInput => steerUserInputDamped;
    private float gasUserInput;
    private float gasUserInputDamped;
    public float GasUserInput => gasUserInputDamped;
    private float brakeUserInput;
    public float BrakeUserInput => brakeUserInput;

    void OnEnable()
    {
        inputAction = new CarControlActionAsset();
        inputAction.Enable();
        inputAction.Steer.Wheel.started += steerPreformed;
        inputAction.Steer.Wheel.canceled += steerPreformed;
        inputAction.Steer.Wheel.performed += steerPreformed;

        inputAction.Pedal.Gas.started += gasPreformed;
        inputAction.Pedal.Gas.canceled += gasPreformed;
        
        inputAction.Pedal.Brake.started += brakePreformed;
        inputAction.Pedal.Brake.canceled += brakePreformed;
    }

    void OnDisable()
    {
        inputAction.Disable();
        inputAction.Steer.Wheel.started -= steerPreformed;
        inputAction.Steer.Wheel.canceled -= steerPreformed;
        inputAction.Steer.Wheel.performed -= steerPreformed;

        inputAction.Pedal.Gas.started -= gasPreformed;
        inputAction.Pedal.Gas.canceled -= gasPreformed;

        inputAction.Pedal.Brake.started -= brakePreformed;
        inputAction.Pedal.Brake.canceled -= brakePreformed;
    }

    void steerPreformed(InputAction.CallbackContext context) {
        steerUserInput = -context.ReadValue<float>();
    }

    void gasPreformed(InputAction.CallbackContext context) {
        gasUserInput = context.ReadValue<float>();
    }

    void brakePreformed(InputAction.CallbackContext context) {
        brakeUserInput = context.ReadValue<float>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (steerUserInput == 0) {
            if (Mathf.Abs(steerUserInputDamped) < Time.deltaTime) steerUserInputDamped = 0;
            else {
                steerUserInputDamped -= Mathf.Sign(steerUserInputDamped) * Time.deltaTime * 5f;
            }
        }
        else steerUserInputDamped = Mathf.Clamp(steerUserInputDamped + steerUserInput * 5f * Time.deltaTime, -1, 1);
        
        gasUserInputDamped = Mathf.Lerp(gasUserInput, gasUserInputDamped, Mathf.Exp(-Time.deltaTime * 10f));
    }
}
