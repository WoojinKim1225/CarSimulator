using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluidDynamicsHelper {
    public class FluidDynamics
    {
        public float Convection(float T_k, float T_0, float h, float dt) {
            return T_k - h * (T_k - T_0) * dt;
        }

        public float fluidDampener(float i, float d, float dt) {
            return Mathf.Lerp(d, i, Mathf.Exp(-dt * 8f));
        }
    }
}