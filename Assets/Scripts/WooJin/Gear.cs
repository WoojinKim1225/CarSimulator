using UnityEngine;

public class Gear
{
    public bool isInput;
    public bool isOutput;
    public float angularVelocity = 0; // rpm
    public float radius {get; private set;} // [m]

    public Gear(float radius) {
        this.radius = radius;
    }
}