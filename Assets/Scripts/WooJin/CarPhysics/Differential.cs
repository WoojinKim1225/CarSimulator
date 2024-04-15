using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differential : MonoBehaviour
{
    public float wInput;
    public enum EDriveTrain {
        Null = 0, FWD = 1, BWD = 2, AWD = 3
    };
    public EDriveTrain driveTrain;
    [SerializeField] private Wheel FL, FR, BL, BR;

    private void Update() {
        if (((int)driveTrain & 1) == 1) {
            FL.w = wInput;
            FR.w = wInput;
        }

        if ((((int)driveTrain >> 1) & 1) == 1) {
            BL.w = wInput;
            BR.w = wInput;
        }
    }
}
