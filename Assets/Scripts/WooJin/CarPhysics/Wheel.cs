using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerCarInput input;

    public float restLength;
    public float springTravel;

    public float k;
    public float c;

    private float minLength;
    private float maxLength;
    private float springLength;
    private float springForceSize;

    private Vector3 suspensionForceWS;

    [Header("Wheel")]
    public float wheelRadius;

    public float staticFrictionCoefficient;
    public float kineticFrictionCoefficient;
    public float rollingFrictionCoefficient;

    public float staticSideFrictionCoefficient;
    public float kineticSideFrictionCoefficient;

    private Vector3 sphereCastStart;
    private Vector3 wheelCenter;
    private float wheelRotationSpeed;
    private float xBefore;
    private Transform wheelMesh;
    private float normalForce;

    private float Fx, Fy;

    private Vector3 wheelVelocityOS;

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
            suspensionForceWS = springForceSize * transform.up;

            wheelVelocityOS = transform.InverseTransformDirection(_rb.GetPointVelocity(hit.point)); // 접촉면의 속도
            normalForce = math.dot(suspensionForceWS, hit.normal);
            Vector3 normalForceWS = normalForce * hit.normal; // 수직항력

            Debug.DrawRay(hit.point, normalForceWS * 0.001f, Color.green);

            PlayerCarControl();

            _rb.AddForceAtPosition(normalForceWS + (Fx * transform.forward) + (Fy * transform.right), transform.position);
            Debug.DrawRay(hit.point, Fx * transform.forward * 0.001f, Color.blue);
            Debug.DrawRay(hit.point, Fy * transform.right * 0.001f, Color.red);
            
            xBefore = x;
        } else {
            wheelCenter = sphereCastStart - transform.up * maxLength;
            springLength = maxLength;
            wheelMesh.position = wheelCenter;
        }

        wheelMesh.rotation *= Quaternion.AngleAxis(wheelRotationSpeed * Time.fixedDeltaTime, Vector3.left);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wheelCenter, wheelRadius);
    }

    void PlayerCarControl()
    {
        if (Mathf.Abs(wheelVelocityOS.x) > 0.1f) {
            Fy = -Mathf.Sign(wheelVelocityOS.x) * normalForce * kineticSideFrictionCoefficient;
        } else {
            Fy = -wheelVelocityOS.x * normalForce * staticSideFrictionCoefficient;
        }

        if (input.BrakeUserInput > 0)
        {
            // 브레이크 밟음
            wheelRotationSpeed = 0;
            if (Mathf.Abs(wheelVelocityOS.z) > 1f)
            {
                Fx = -Mathf.Sign(wheelVelocityOS.z) * normalForce * kineticFrictionCoefficient;
            }
            else
            {
                Fx = -wheelVelocityOS.z * normalForce * staticFrictionCoefficient;
            }
            return;
        }

        if (input.GasUserInput > 0)
        {

            Fx = input.GasUserInput * normalForce;
            wheelRotationSpeed += input.GasUserInput * Time.fixedDeltaTime;
        }
        else
        {
            Fx = 0;
        }
    }
}
