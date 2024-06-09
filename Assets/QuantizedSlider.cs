using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class QuantizedSlider : MonoBehaviour
{
    public XRSlider xRSlider;
    public XRSliderBinning xRSliderBinning;
    public float maxPosition, minPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Cos(xRSlider.value * Mathf.PI * 2f * (xRSliderBinning.range - 1f)) < 0) return;

        transform.localPosition = Mathf.Lerp(minPosition, maxPosition, bin(xRSlider.value)) * Vector3.forward;
    }

    private float bin(float value) {
        return Mathf.Round(value * (xRSliderBinning.range - 1)) / (xRSliderBinning.range - 1);
    }
}
