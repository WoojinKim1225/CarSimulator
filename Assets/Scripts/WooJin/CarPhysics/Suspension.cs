using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private Wheel wheel;
    [SerializeField] private float restLength = 0.4f;
    [SerializeField] private float springTravel = 0.1f;

    [SerializeField] private float k = 40000;
    [SerializeField] private float c = 4000;

    private float _minLength, _maxLength;
    private float _springLength;
    private float _springForceSize;

    //private Vector3 suspensionForceWS => _springForceSize * transform.up;
    private float wheelRadius => wheel.radius;

    private Vector3 sphereCastStartPositionWS;
    private Vector3 wheelCenter;

    private float xBefore;
    private Transform wheelMesh;


    public Vector3 suspensionForceWS, weightForceWS;
    public Vector3 hitPositionWS, hitNormalWS;

    private Vector3 Vector3NaN = new Vector3(float.NaN, float.NaN, float.NaN);

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();

        _minLength = restLength - springTravel;
        _maxLength = restLength + springTravel;
        wheelMesh = transform.GetChild(0);
    }

    private void Update() {
        Debug.DrawRay(sphereCastStartPositionWS, -transform.up * (_springLength + wheelRadius), Color.green);
    }

    private void FixedUpdate() {
        sphereCastStartPositionWS = transform.position + transform.up * _minLength;
        
        if (Physics.SphereCast(sphereCastStartPositionWS, wheelRadius, -transform.up, out RaycastHit hit, _maxLength)) {
            wheelCenter = hit.point + hit.normal * wheelRadius; // 바퀴 축 위치
            wheelMesh.position = wheelCenter;

            _springLength = Vector3.Distance(sphereCastStartPositionWS, wheelCenter); // 스프링의 길이

            float x = restLength - _springLength; // 스프링 변위
            float v = (x - xBefore) / Time.fixedDeltaTime; // 스프링 속도
            _springForceSize = k * x + c * v;

            suspensionForceWS = _springForceSize * transform.up; 
            weightForceWS = rb.mass * Physics.gravity * 0.25f;
            hitPositionWS = hit.point;
            hitNormalWS = hit.normal;
            
            Debug.DrawRay(hit.point, suspensionForceWS * 0.0000001f, Color.green);
            Debug.DrawRay(hit.point, weightForceWS * 0.0001f, Color.red);
            
            //_rb.AddForceAtPosition(normalForceWS, transform.position);
            
            wheel.groundVelocityOS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point)); // 접촉면의 속도

            
            xBefore = x;
        } else {
            wheelCenter = sphereCastStartPositionWS - transform.up * _maxLength;
            _springLength = _maxLength;
            wheelMesh.position = wheelCenter;
            suspensionForceWS = Vector3.zero;
            hitPositionWS = Vector3NaN;
            hitNormalWS = Vector3NaN;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wheelCenter, wheelRadius);
    }
}
