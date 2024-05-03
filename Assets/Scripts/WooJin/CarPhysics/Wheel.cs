using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Suspension suspension;

    public float radius;
    public float angularVelocity;

    private Transform wheelMesh;
    public Vector3 groundVelocityOS;
    public Vector3 hitPositionWS;
    public Vector3 hitNormalWS;
    public bool isWheelLocked;
    public float staticFrictionCoefficient;
    public AnimationCurve fc_DryAsphalt, fc_WetAsphalt;

    private void Awake() {
        wheelMesh = transform.GetChild(0).transform;
    }
    private void FixedUpdate() {
        if (suspension.hitPositionWS.x == float.NaN) return;
        suspension.rb.AddForceAtPosition(suspension.suspensionForceWS, suspension.hitPositionWS);
        if (isWheelLocked) {
            if (staticFrictionCoefficient * Vector3.Dot(suspension.suspensionForceWS, hitNormalWS) > Vector3.Magnitude(Vector3.ProjectOnPlane(suspension.suspensionForceWS, hitNormalWS))) {
                //suspension.rb.AddForceAtPosition(-Vector3.ProjectOnPlane(suspension.suspensionForceWS, hitNormalWS), hitPositionWS);
            }
        }
    }
}
