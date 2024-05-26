using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerCarVRInput : MonoBehaviour
{
    public InputActionReference accel, brake;
    public Wheel fl, fr, bl, br;
    private Wheel[] wheels => new Wheel[4] {fl, fr, bl, br};
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
            foreach (var wheel in wheels) {
                wheel.isBrake = true;
            }
        } else if (accel.action.ReadValue<float>() > 0) {
            engine.throttlePosition = accel.action.ReadValue<float>();
            foreach (var wheel in wheels) {
                wheel.isBrake = false;
            }
        } else {
            engine.throttlePosition = 0;
            foreach (var wheel in wheels) {
                wheel.isBrake = false;
            }
        }
    }
}
