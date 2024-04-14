using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidDynamicsHelper : MonoBehaviour
{
    public float Convection(float T_k, float T_0, float h, float dt) {
        return T_k - h * (T_k - T_0) * dt;
    }
}
