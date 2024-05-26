using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    public AudioSource engineIdleAudioSource, engineStartAudioSource;
    public Engine engine;
    public int beforeEngineState;


    void Update()
    {
        if (engine.State == (int)EEngineState.Start && beforeEngineState != (int)EEngineState.Start) {
            engineIdleAudioSource.Play();
            engineStartAudioSource.Play();
        }
        if (engine.State == (int)EEngineState.On && beforeEngineState == (int)EEngineState.On) {
            engineStartAudioSource.Stop();
        }
        if (engine.State < (int)EEngineState.On) {
            engineIdleAudioSource.Stop();
        }
        beforeEngineState = engine.State;
        float pitch = Mathf.Lerp(0, 3, Mathf.InverseLerp(0, 8500, engine.currentEngineRPM));
        engineIdleAudioSource.pitch = pitch;
    }
}
