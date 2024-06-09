using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class HandBrake : MonoBehaviour
{
    public XRLever xRLever;
    public List<Wheel> wheels = new List<Wheel>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Wheel wheel in wheels){
            wheel.isHandBrake = !xRLever.value;
        }
    }
}
