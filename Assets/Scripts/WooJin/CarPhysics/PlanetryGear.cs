using UnityEditor;
using UnityEngine;

public class PlanetryGear
{
    public Gear Sun, Ring, Planet;

    public PlanetryGear(float sunRadius, float ringRadius) {
        if (ringRadius <= 0) {
            Debug.LogError("ring gear radius must be positive!");
            return;
        }

        if (sunRadius <= 0) {
            Debug.LogError("sun gear radius must be positive!");
            return;
        }

        if (ringRadius <= sunRadius) {
            Debug.LogError("ring gear radius must be bigger than the sun gear radius!");
            return;
        }
        Sun = new Gear(sunRadius);
        Ring = new Gear(ringRadius);
        Planet = new Gear(ringRadius - sunRadius);
    }

    public void OnUpdate()
    {
        //Ring.angularVelocity * Ring.radius = Planet.angularVelocity * (Ring.radius + Sun.radius) - Sun.angularVelocity * Sun.radius;
        if (Sun.isInput && Planet.isInput) {
            Ring.isInput = false;
            Ring.isOutput = true;
            Ring.angularVelocity = (Planet.angularVelocity * (Ring.radius + Sun.radius) - Sun.angularVelocity * Sun.radius) / Ring.radius;
        }
        else if (Sun.isInput && Ring.isInput) {
            Planet.isInput = false;
            Planet.isOutput = true;
            Planet.angularVelocity = (Ring.angularVelocity * Ring.radius + Sun.angularVelocity * Sun.radius) / (Ring.radius + Sun.radius);
        }
        else if (Planet.isInput && Ring.isInput) {
            Sun.isInput = false;
            Sun.isOutput = true;
            Sun.angularVelocity = (Planet.angularVelocity * (Ring.radius + Sun.radius) - Ring.angularVelocity * Ring.radius) / Sun.radius;
        } else {
            Sun.isOutput = false;
            Planet.isOutput = false;
            Ring.isOutput = false;
            Sun.angularVelocity = 0f;
            Planet.angularVelocity = 0f;
            Ring.angularVelocity = 0f;
        }

    }
}