using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControl : MonoBehaviour
{   
    public Rigidbody _rb;
    public PlayerCarInput input;
    public Wheel wheelFL, wheelFR, wheelBL, wheelBR;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnCurvature;

    private float ackermannAngleLeft, ackermannAngleRight;

    public float speed;

    void Start()
    {
        wheelBase = Vector3.Distance(wheelFL.transform.position, wheelBL.transform.position);
        rearTrack = Vector3.Distance(wheelBL.transform.position, wheelBR.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan2(2 * wheelBase * turnCurvature, 2 + Mathf.Sign(input.SteerUserInput) * rearTrack * turnCurvature) * input.SteerUserInput;
        ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan2(2 * wheelBase * turnCurvature, 2 - Mathf.Sign(input.SteerUserInput) *  rearTrack * turnCurvature) * input.SteerUserInput;

        wheelFL.transform.localRotation = Quaternion.Euler(0, ackermannAngleLeft, 0);
        wheelFR.transform.localRotation = Quaternion.Euler(0, ackermannAngleRight, 0);

        speed = Vector3.Dot(_rb.velocity, transform.forward) * 3.6f;
    }
}
