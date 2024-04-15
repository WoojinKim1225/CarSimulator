using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Wheel wheel;
    //[SerializeField] private PlayerCarInput input;

    public float restLength;
    public float springTravel;

    public float k;
    public float c;

    private float minLength;
    private float maxLength;
    private float springLength;
    private float springForceSize;

    private Vector3 suspensionForceWS => springForceSize * transform.up;

    private float wheelRadius => wheel.radius;

    public AnimationCurve fc_DryAsphalt, fc_WetAsphalt;

    private Vector3 sphereCastStart;
    private Vector3 wheelCenter;

    private float xBefore;
    private Transform wheelMesh;
    private float normalForce;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
        wheelMesh = transform.GetChild(0);
    }

    private void Update() {
        Debug.DrawRay(sphereCastStart, -transform.up * (springLength + wheelRadius), Color.green);
    }

    private void FixedUpdate() {
        sphereCastStart = transform.position + transform.up * minLength;
        
        if (Physics.SphereCast(sphereCastStart, wheelRadius, -transform.up, out RaycastHit hit, maxLength)) {
            wheelCenter = hit.point + hit.normal * wheelRadius; // 바퀴 축 위치
            wheelMesh.position = wheelCenter;

            springLength = Vector3.Distance(sphereCastStart, wheelCenter); // 스프링의 길이

            float x = restLength - springLength; // 스프링 변위
            float v = (x - xBefore) / Time.fixedDeltaTime; // 스프링 속도
            springForceSize = k * x + c * v; 

            normalForce = math.dot(suspensionForceWS, hit.normal);
            Vector3 normalForceWS = normalForce * hit.normal; // 수직항력
            
            _rb.AddForceAtPosition(normalForceWS, transform.position);
            
            wheel.groundVelocityOS = transform.InverseTransformDirection(_rb.GetPointVelocity(hit.point)); // 접촉면의 속도

            Debug.DrawRay(hit.point, normalForceWS * 0.001f, Color.green);
            
            xBefore = x;
        } else {
            wheelCenter = sphereCastStart - transform.up * maxLength;
            springLength = maxLength;
            wheelMesh.position = wheelCenter;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wheelCenter, wheelRadius);
    }
}
