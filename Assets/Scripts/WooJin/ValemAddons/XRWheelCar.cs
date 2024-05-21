using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRWheelCar : MonoBehaviour
{
    public XRKnob wheel;
    public CarControl carControl;

    void Update()
    {
        carControl.steerUserInput = (wheel.value - 0.5f) * 2f;
    }
}
