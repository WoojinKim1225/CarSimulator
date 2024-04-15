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
    
    /*
    private bool c1 => (clutchMode & 1) == 1;
    private bool c2 => ((clutchMode >> 1) & 1) == 1;
    private bool c3 => ((clutchMode >> 2) & 1) == 1;
    private bool c4 => ((clutchMode >> 3) & 1) == 1;
    private bool c5 => ((clutchMode >> 4) & 1) == 1;
    */

    public bool c1;
    public bool c2;
    public bool c3;
    public bool c4;
    public bool c5;


    public float wOutput;
    public float sun, planet;

    public PlanetryGear c3PGear, c4PGear, c5PGear;

    void Awake()
    {
        c3PGear = new PlanetryGear(1f, 3.1f);
        c4PGear = new PlanetryGear(1f, 3.1f);
        c5PGear = new PlanetryGear(1f, 3.1f);
    }

    void OnDestroy()
    {
        c3PGear = null;
        c4PGear = null;
        c5PGear = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        sun = c1 ? inputRPM : 0f;
        planet = c2 ? inputRPM : 0f;

        c3PGear.Sun.isInput = true;
        c3PGear.Sun.angularVelocity = inputRPM;
        c4PGear.Sun.isInput = true;
        c4PGear.Sun.angularVelocity = sun;
        c5PGear.Sun.isInput = true;
        c5PGear.Sun.angularVelocity = sun;

        if (c3) {
            c3PGear.Ring.isInput = true;
            c3PGear.Ring.angularVelocity = 0;
            c4PGear.Ring.angularVelocity = c3PGear.Planet.angularVelocity;
        }else {
            c3PGear.Ring.isInput = false;
        }
        
        
        if (c4) {
            c4PGear.Ring.isInput = true;
            c4PGear.Ring.angularVelocity = 0;
            c5PGear.Ring.angularVelocity = c4PGear.Planet.angularVelocity;
        }

        if (c5) {
            c5PGear.Ring.isInput = true;
            c5PGear.Ring.angularVelocity = 0;
        }
        if (c2) {
            c5PGear.Ring.isInput = true;
            c5PGear.Ring.angularVelocity = planet;
        }

        c3PGear.OnUpdate();
        c4PGear.OnUpdate();
        c5PGear.OnUpdate();
        wOutput = c5PGear.Planet.angularVelocity;
    }

    public float PlanetryGear_planet(float wSun, float wRing) {
        return (wSun + 2.1f * wRing) / 3.1f;
    }

    public float PlanetryGear_sun(float wPlanet, float wRing) {
        return 3.1f * wPlanet - 2.1f * wRing;
    }
}
