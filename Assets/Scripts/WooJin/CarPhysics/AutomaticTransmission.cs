using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluidDynamicsHelper;


public enum EGear
{
    Parking = 0, Reverse = 1, Neutral = 2, Drive = 3, Manual = 4, Sport = 5, Winter = 6, SAFEMODE = 7
}
public class AutomaticTransmission : MonoBehaviour
{
    public float inputRPM;
    public float torqueConvertedRPM;

    public EGear gear;
    public int clutchNumber;
    private readonly Dictionary<int, int> numberToMode = new Dictionary<int, int>{
        { -1, 20 },
        { 0, 0 },
        { 1, 17 },
        { 2, 9},
        { 3, 5 },
        { 4, 3 },
        { 5, 6 },
        { 6, 10 },
    };
    private int clutchMode => numberToMode.GetValueOrDefault(clutchNumber);
    
    
    private bool c1 => (clutchMode & 1) == 1;
    private bool c2 => ((clutchMode >> 1) & 1) == 1;
    private bool c3 => ((clutchMode >> 2) & 1) == 1;
    private bool c4 => ((clutchMode >> 3) & 1) == 1;
    private bool c5 => ((clutchMode >> 4) & 1) == 1;


    public float wOutput;
    public bool wIsPowered;

    private PlanetryGear c3PGear, c4PGear, c5PGear;

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
        torqueConvertedRPM = TorqueConverter(inputRPM, torqueConvertedRPM, Time.deltaTime);

        GearToClutchMode();

        c3PGear.Sun.isInput = false;
        c3PGear.Ring.isInput = false;
        c3PGear.Planet.isInput = false;
        c4PGear.Sun.isInput = false;
        c4PGear.Ring.isInput = false;
        c4PGear.Planet.isInput = false;
        c5PGear.Sun.isInput = false;
        c5PGear.Ring.isInput = false;
        c5PGear.Planet.isInput = false;

        if (c1) {
            c4PGear.Sun.isInput = true;
            c4PGear.Sun.angularVelocity = 1;

            c5PGear.Sun.isInput = true;
            c5PGear.Sun.angularVelocity = 1;
        }

        if (c2) {
            c4PGear.Planet.isInput = true;
            c4PGear.Planet.angularVelocity = 1;

            c5PGear.Ring.isInput = true;
            c5PGear.Ring.angularVelocity = 1;
        }

        c3PGear.Sun.isInput = true;
        c3PGear.Sun.angularVelocity = 1;

        if (c3) {
            c3PGear.Ring.isInput = true;
            c3PGear.Ring.angularVelocity = 0;
        }

        if (c3PGear.Planet.isOutput) {
            c4PGear.Ring.isInput = true;
            c4PGear.Ring.angularVelocity = c3PGear.Planet.angularVelocity;
        }

        if (c4) {
            c4PGear.Ring.isInput = true;
            c4PGear.Ring.angularVelocity = 0;
        }

        if (c4PGear.Planet.isOutput) {
            c5PGear.Ring.isInput = true;
            c5PGear.Ring.angularVelocity = c4PGear.Planet.angularVelocity;
        }

        if (c4PGear.Sun.isOutput) {
            c5PGear.Sun.isInput = true;
            c5PGear.Sun.angularVelocity = c4PGear.Sun.angularVelocity;
        }

        if (c5) {
            c5PGear.Ring.isInput = true;
            c5PGear.Ring.angularVelocity = 0;

            c4PGear.Planet.isInput = true;
            c4PGear.Planet.angularVelocity = 0;
        }

        c3PGear.OnUpdate();
        c4PGear.OnUpdate();
        c5PGear.OnUpdate();

        wOutput = Mathf.Lerp(c5PGear.Planet.angularVelocity * torqueConvertedRPM, wOutput, Mathf.Exp(-Time.deltaTime * 20f));
        wIsPowered = c5PGear.Planet.isOutput;
    }

    void GearToClutchMode() {
        switch (gear)
        {
            case EGear.Parking:
                break;
            case EGear.Reverse:
                clutchNumber = -1;
                break;
            case EGear.Neutral:
                clutchNumber = 0;
                break;
            case EGear.Drive:
                break;
            case EGear.Manual:
                break;
            default:
                break;
        }
    }

    float TorqueConverter (float input, float convert, float dt) {
        return convert + (input - convert) * 0.1f * dt;
    }
}
