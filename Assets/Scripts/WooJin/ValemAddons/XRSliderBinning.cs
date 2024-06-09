using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRSliderBinning : MonoBehaviour
{
    XRSlider slider;
    private float before;
    public AutomaticTransmission automaticTransmission;
    public AudioRandomPlayer audioRandomPlayer;

    public float range = 4;

    void Awake()
    {
        slider = GetComponent<XRSlider>();
    }

    public void SetSliderBinning() {
        slider.value = bin(slider.value);
    }

    void Update()
    {
        if (Mathf.Cos(slider.value * Mathf.PI * 2f * (range - 1f)) < 0) return;
        automaticTransmission.SetGear((int)(slider.value * (range - 1)));
        if (bin(slider.value) != bin(before)) {
            audioRandomPlayer.PlayRandom();
            before = slider.value;
        }
    }

    private float bin(float value) {
        return Mathf.Round(value * (range - 1)) / (range - 1);
    }
}
