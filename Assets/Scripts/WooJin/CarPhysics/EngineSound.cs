using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    AudioSource audioSource;
    public Engine engine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (engine.State == (int)EEngineState.Start) {
            audioSource.Play();
        }
        if (engine.State < (int)EEngineState.On) {
            audioSource.Stop();
        }
        float pitch = Mathf.Lerp(0, 3, Mathf.InverseLerp(0, 8500, engine.currentEngineRPM));
        audioSource.pitch = pitch;
    }
}
