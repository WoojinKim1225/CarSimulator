using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differential : MonoBehaviour
{
    public float rpmInput;

    public enum EDriveTrain {Null = 0, FWD = 1, BWD = 2, AWD = 3};

    public EDriveTrain driveTrain;

    [SerializeField] private Wheel FL, FR, BL, BR;

    private void Update() {
        if (((int)driveTrain & 1) == 1) {
            FL.rpm = rpmInput;
            FR.rpm = rpmInput;
            FL.isPowered = true;
            FR.isPowered = true;
        } else {
            FL.isPowered = false;
            FR.isPowered = false;
        }

        if ((((int)driveTrain >> 1) & 1) == 1) {
            BL.rpm = rpmInput;
            BR.rpm = rpmInput;
            BL.isPowered = true;
            BR.isPowered = true;
        } else {
            BL.isPowered = false;
            BR.isPowered = false;
        }
    }
}
