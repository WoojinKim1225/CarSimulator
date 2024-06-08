using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRDialCarStarter : MonoBehaviour
{
    public XRKnob knob;
    public Engine engine;
    
    void Update()
    {
        engine.State = (int)(knob.value * 3f);
    }

    public void SetKnobBinning() {
        float f = Mathf.Min(0.75f, knob.value);
        knob.value = Mathf.Floor(f * 3f + 0.5f) / 3f;
    }
}
