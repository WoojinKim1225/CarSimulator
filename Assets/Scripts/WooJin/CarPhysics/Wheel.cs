using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Rigidbody rb;
    public float radius;
    public float rpm;

    private float angle;

    private Transform wheelMesh;
    public Vector3 groundVelocityOS, groundVelocityWS;
    public bool isPowered;
    public bool isBrake;

    public float staticFrictionCoefficient;
    public AnimationCurve fc_DryAsphalt, fc_WetAsphalt;

    [SerializeField] private Vector2 givenVelocity, appliedVelocity;
    public Vector3 hitPosition, hitNormal;
    public Vector3 normalForceWS;
    public float normalForce;

    public float slip;
    public float frictionCoefficient;

    public float f;

    private void Awake() {
        wheelMesh = transform.GetChild(0).transform;
    }

    private void Update() {
        angle += rpm * 6 * Time.deltaTime;
        angle %= 360f;
        wheelMesh.localRotation = Quaternion.Euler(angle, 0, 0);
        givenVelocity.y = radius * rpm * 6 * Mathf.Deg2Rad;
        if (hitNormal != Vector3.zero) {
            Vector3 biTangent = Quaternion.FromToRotation(transform.up, hitNormal) * transform.right;
            Vector3 Tangent = Quaternion.FromToRotation(transform.up, hitNormal) * transform.forward;
        
            appliedVelocity.x = Vector3.Dot(biTangent, groundVelocityWS);
            appliedVelocity.y = Vector3.Dot(Tangent, groundVelocityWS);

            slip = Mathf.Clamp01((appliedVelocity - givenVelocity).magnitude / givenVelocity.magnitude);
            if (givenVelocity.magnitude < 0.1f) frictionCoefficient = staticFrictionCoefficient * appliedVelocity.magnitude;
            else frictionCoefficient = fc_DryAsphalt.Evaluate(slip);
            //rb.AddForceAtPosition(frictionCoefficient * normalForce * (-appliedVelocity.x * biTangent - appliedVelocity.y * Tangent).normalized, hitPosition);

            if (!isBrake) {
                if (isPowered) {
                    rb.AddForceAtPosition(frictionCoefficient * f * normalForce * (-appliedVelocity.x * biTangent + (- appliedVelocity.y + givenVelocity.y) * Tangent), hitPosition);
                } else {
                    rb.AddForceAtPosition(frictionCoefficient * f  * normalForce * (-appliedVelocity.x * biTangent), hitPosition);
                }
            } else {
                rb.AddForceAtPosition(frictionCoefficient * f * normalForce * (-appliedVelocity.x * biTangent - appliedVelocity.y * Tangent), hitPosition);
            }

            Debug.DrawRay(hitPosition, biTangent, Color.red);
            Debug.DrawRay(hitPosition, Tangent, Color.blue);
            //Debug.DrawRay(hitPosition, frictionCoefficient * normalForce * (-appliedVelocity.x * biTangent - appliedVelocity.y * Tangent).normalized, Color.green);
            Debug.DrawRay(hitPosition, 100f * (-appliedVelocity.x * biTangent + (- appliedVelocity.y + givenVelocity.y) * Tangent), Color.green);
        }
    }
}
