using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSliderBinning : MonoBehaviour
{
    XRSlider slider;
    private float before;
    public AutomaticTransmission automaticTransmission;
    public AudioRandomPlayer audioRandomPlayer;
    public Transform quantizedSlider;

    public XRBaseController xRBaseController;

    public float range = 4;
    private EGear beforeGear;

    void Awake()
    {
        slider = GetComponent<XRSlider>();
    }

    public void SetSliderBinning() {
        slider.value = bin(slider.value);
    }
    void Update()
    {
        if (Mathf.Abs(bin(slider.value) - slider.value) > 0.05f) {
            return;
        }
        if (automaticTransmission.GetGear() != beforeGear) {
            beforeGear = automaticTransmission.GetGear();
            xRBaseController.SendHapticImpulse(1, 0.2f);
        }
        quantizedSlider.localPosition = Mathf.Lerp(-0.06f, 0.06f, bin(slider.value)) * Vector3.forward;

        automaticTransmission.SetGear((int)(bin(slider.value) * 4f));
        if (bin(slider.value) != bin(before)) {
            audioRandomPlayer.PlayRandom();
            before = slider.value;
        }
    }

    private float bin(float value) {
        return Mathf.Round(value * (range - 1)) / (range - 1);
    }
}
