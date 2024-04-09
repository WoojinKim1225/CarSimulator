using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{
    private Transform _wheelHolderTransform;
    private Transform _wheelTransform;

    [SerializeField] private Rigidbody carRigidbody;

    [SerializeField] private Vector3 _springAxis = Vector3.up;
    [SerializeField] private Vector3 _rotationAxis = Vector3.left;

    [SerializeField] private float _tireRadius;
    [SerializeField] private float _tireWidth;

    [SerializeField] private LayerMask whatIsCollideWithWheel;

    [SerializeField] private float _springOrigin;
    [SerializeField] private float _idleHeight;
    [SerializeField] private float _k;
    [SerializeField] private float _c;

    //RaycastHit[] results;
    private float xBefore;

    void Awake()
    {
        // Instantiate wheel transform;
        _wheelHolderTransform = transform;
        _wheelTransform = _wheelHolderTransform.GetChild(0);
    }

    private void FixedUpdate() {
        Vector3 springAxisWS = transform.TransformDirection(_springAxis);
        
        bool isCollide = Physics.SphereCast(_wheelHolderTransform.position + springAxisWS * _springOrigin, 0.3f, -Vector3.up, out RaycastHit results, -_idleHeight + _springOrigin);
        float x;
        //if (collideCount <= 0) return;
        if (isCollide) {
            x = Vector3.Dot(springAxisWS, results.point + results.normal * _tireRadius - transform.TransformPoint(_idleHeight * Vector3.up));
            float xDot = (x - xBefore) / Time.fixedDeltaTime;
            Debug.Log(x);

            float force = _k * x + _c * xDot;
            force = math.max(force, 0);
            carRigidbody.AddForceAtPosition(force * springAxisWS, transform.position);
            Debug.DrawRay(transform.position, springAxisWS * force, Color.magenta);
        } else {
            x = -_idleHeight + _springOrigin;
        }

        xBefore = x;
    }
}
