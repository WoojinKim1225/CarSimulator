using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpeedParticle : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Engine.Instance.carSpeed * 3.6f;
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = Mathf.Max(speed - 60f, 0);
        
        //particleSystem.emission
    }
}
