using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float radius;
    public float w;

    private Transform wheelMesh;
    public Vector3 groundVelocityOS;
    public float staticFrictionCoefficient;
    public AnimationCurve fc_DryAsphalt, fc_WetAsphalt;

    private void Awake() {
        wheelMesh = transform.GetChild(0).transform;
    }
    private void Update() {
        //wheelMesh.rotation *= ;
    }
}
