using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRSliderBinning : MonoBehaviour
{
    XRSlider slider;
    public AutomaticTransmission automaticTransmission;

    public float range = 4;

    void Awake()
    {
        slider = GetComponent<XRSlider>();
    }

    public void SetSliderBinning() {
        slider.value = Mathf.Floor(slider.value * (range - 1) + 0.5f) / (range - 1);
    }

    void Update()
    {
        automaticTransmission.gear = (EGear)(int)(slider.value * (range - 1));
    }
}
