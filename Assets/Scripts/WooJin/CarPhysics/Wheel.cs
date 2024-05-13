using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float radius;
    public float rpm;

    private float angle;

    private Transform wheelMesh;
    public Vector3 groundVelocityOS, groundVelocityWS;
    public float staticFrictionCoefficient;
    public AnimationCurve fc_DryAsphalt, fc_WetAsphalt;

    [SerializeField] private Vector2 givenVelocity, appliedVelocity;
    public Vector3 hitPosition, hitNormal;

    private void Awake() {
        wheelMesh = transform.GetChild(0).transform;
    }

    private void Update() {
        angle += rpm * 6 * Time.deltaTime;
        angle %= 360f;
        wheelMesh.rotation = Quaternion.Euler(angle, 0, 0);
        givenVelocity.y = radius * rpm * 6 * Mathf.Deg2Rad;
        if (hitNormal != Vector3.zero) {
            Vector3 biTangent = Quaternion.FromToRotation(transform.up, hitNormal) * transform.right;
            Vector3 Tangent = Quaternion.FromToRotation(transform.up, hitNormal) * transform.up;
        
            appliedVelocity.x = Vector3.Dot(biTangent, groundVelocityWS);
            appliedVelocity.y = Vector3.Dot(Tangent, groundVelocityWS);
        }
    }
}
