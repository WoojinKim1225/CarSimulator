using UnityEditor;
using UnityEngine;

public class PlanetryGear : MonoBehaviour
{
    public Gear Sun, Ring, Planet;

    public PlanetryGear(float sunRadius, float ringRadius) {
        if (ringRadius <= 0) {
            Debug.LogError(this.name + "'s ring gear radius must be positive!");
            return;
        }

        if (sunRadius <= 0) {
            Debug.LogError(this.name + "'s sun gear radius must be positive!");
            return;
        }

        if (ringRadius <= sunRadius) {
            Debug.LogError(this.name + "'s ring gear radius must be bigger than the sun gear radius!");
            return;
        }
        Sun = new Gear(sunRadius);
        Ring = new Gear(ringRadius);
        Planet = new Gear(ringRadius - sunRadius);
    }

    void Update()
    {
        //Ring.angularVelocity * Ring.radius = Planet.angularVelocity * (Ring.radius + Sun.radius) - Sun.angularVelocity * Sun.radius;

        if (Sun.isPowered && Planet.isPowered && !Ring.isPowered) {
            Ring.angularVelocity = (Planet.angularVelocity * (Ring.radius + Sun.radius) - Sun.angularVelocity * Sun.radius) / Ring.radius;
        }
        else if (Sun.isPowered && !Planet.isPowered && Ring.isPowered) {
            Planet.angularVelocity = (Ring.angularVelocity * Ring.radius + Sun.angularVelocity * Sun.radius) / (Ring.radius + Sun.radius);
        }
        else if (!Sun.isPowered && Planet.isPowered && Ring.isPowered) {
            Sun.angularVelocity = (Planet.angularVelocity * (Ring.radius + Sun.radius) - Ring.angularVelocity * Ring.radius) / Sun.radius;
        }

    }
}