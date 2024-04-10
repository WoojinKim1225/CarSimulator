using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControl : MonoBehaviour
{   
    public Rigidbody _rb;
    public CarControlActionAsset inputAction;
    public Wheel wheelFL, wheelFR, wheelBL, wheelBR;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnCurvature;

    [Header("Inputs")]
    private float steerInput;
    private float steerInputDamped;
    private float gasInput;
    private float gasInputDamped;
    public float GasInput => gasInputDamped;
    private float brakeInput;
    public float BrakeInput => brakeInput;

    private float ackermannAngleLeft, ackermannAngleRight;

    public float speed;

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
        steerInput = -context.ReadValue<float>();
    }

    void gasPreformed(InputAction.CallbackContext context) {
        gasInput = context.ReadValue<float>();
    }

    void brakePreformed(InputAction.CallbackContext context) {
        brakeInput = context.ReadValue<float>();
    }

    void Start()
    {
        wheelBase = Vector3.Distance(wheelFL.transform.position, wheelBL.transform.position);
        rearTrack = Vector3.Distance(wheelBL.transform.position, wheelBR.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //if (steerInput == 0) steerInputDamped = 0;
        //else steerInputDamped = Mathf.Clamp(steerInputDamped + steerInput * Time.deltaTime, -1, 1);
        steerInputDamped = steerInput;
        gasInputDamped = Mathf.Lerp(gasInput, gasInputDamped, Mathf.Exp(-Time.deltaTime * 10f));
        ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan2(2 * wheelBase * turnCurvature, 2 + Mathf.Sign(steerInputDamped) * rearTrack * turnCurvature) * steerInputDamped;
        ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan2(2 * wheelBase * turnCurvature, 2 - Mathf.Sign(steerInputDamped) *  rearTrack * turnCurvature) * steerInputDamped;

        wheelFL.transform.localRotation = Quaternion.Euler(0, ackermannAngleLeft, 0);
        wheelFR.transform.localRotation = Quaternion.Euler(0, ackermannAngleRight, 0);

        speed = Vector3.Dot(_rb.velocity, transform.forward) * 3.6f;
    }
}
