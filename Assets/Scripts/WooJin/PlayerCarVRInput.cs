using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarVRInput : MonoBehaviour
{
    public InputActionReference accel, brake;
    public Wheel fl, fr, bl, br;
    public Engine engine;


    // Start is called before the first frame update
    void Start()
    {
        accel.action.Enable();
        brake.action.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (brake.action.ReadValue<float>() > 0) {
            engine.throttlePosition = 0;
            fl.isBrake = true;
            fr.isBrake = true;
            bl.isBrake = true;
            br.isBrake = true;
        } else if (accel.action.ReadValue<float>() > 0) {
            engine.throttlePosition = accel.action.ReadValue<float>();
            fl.isBrake = false;
            fr.isBrake = false;
            bl.isBrake = false;
            br.isBrake = false;
        } else {
            engine.throttlePosition = 0;
            fl.isBrake = false;
            fr.isBrake = false;
            bl.isBrake = false;
            br.isBrake = false;
        }
    }
}
