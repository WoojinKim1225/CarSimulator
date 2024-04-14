using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGear
{
    Parking = 0, Reverse = 1, Neutral = 2, Drive = 3, Maunal = 4, Sport = 5, Winter = 6, SAFEMODE = 7
}
public class AutomaticTransmission : MonoBehaviour
{
    public float inputRPM;

    public EGear gear;
    public int clutchMode;
    
    // is fist shaft applied?
    private bool c1 => (clutchMode & 1) == 1;
    private bool c2 => ((clutchMode >> 1) & 1) == 1;
    private bool c3 => ((clutchMode >> 2) & 1) == 1;
    private bool c4 => ((clutchMode >> 3) & 1) == 1;
    private bool c5 => ((clutchMode >> 4) & 1) == 1;


    private float sunSpeed;
    private float wOutput;
    private float c5Ring, c4Ring, c3Ring;

    private PlanetryGear c3PGear, c4PGear, c5PGear;

    void Awake()
    {
        c3PGear = new PlanetryGear(1f, 3.1f);
        c4PGear = new PlanetryGear(1f, 3.1f);
        c5PGear = new PlanetryGear(1f, 3.1f);
    }

    void OnDestroy()
    {
        Destroy(c3PGear);
        Destroy(c4PGear);
        Destroy(c5PGear);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        float ringSpeed;
        if (c1) {
            if (c2) ringSpeed = wInput;
            else ringSpeed = PlanetryGear_planet(wInput, 0f);
            if (c5) {
                ringSpeed = PlanetryGear_planet(wInput, ringSpeed);
            }
        } else {
            if (c4) {

            }
        }

        // 1st gear: 10001
        // 2nd gear: 10010
        // 3rd gear: 10100
        // 4th gear: 11000
        // 5th gear: 01100
        // 6th gear: 01010
        // rev gear: 00101

        float c3Sun = wInput;
        sunSpeed = c1 ? wInput : 0f;

        if (c5) c5Ring = 0;
        if (c2) c5Ring = wInput;
        
        if (c4) c4Ring = 0;// ringSpeed = PlanetryGear_planet(sunSpeed, 0f);
        if (c3) c3Ring = 0;


        c4Ring = PlanetryGear_planet(c3Sun, c3Ring);
        wOutput = PlanetryGear_planet(sunSpeed, c5Ring);
        */

    }

    public float PlanetryGear_planet(float wSun, float wRing) {
        return (wSun + 2.1f * wRing) / 3.1f;
    }

    public float PlanetryGear_sun(float wPlanet, float wRing) {
        return 3.1f * wPlanet - 2.1f * wRing;
    }
}
