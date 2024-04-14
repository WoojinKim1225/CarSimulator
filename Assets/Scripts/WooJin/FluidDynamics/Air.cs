using System.Runtime.Serialization;
using UnityEngine;

public class Air : MonoBehaviour
{
    public float Temperature; // [°C]
    public float Humidity; // [%]
    public bool isFreeze => Temperature <= 0f;
    public float Precipitation; // [mm/H]
    public bool isRaining => Precipitation > Mathf.Epsilon;
    private bool wasRaining;
    public bool isSnowy => isRaining && isFreeze;

    public static Air Instance;

    private float targetHumidity;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        Humidity = .65f;
        Temperature = 20f;
    }

    private void Update() {
        ModifyHumidity();
    }

    

    private void ModifyHumidity() {
        if (isRaining) {
            targetHumidity = 0.65f + (1 - Mathf.Exp(-Precipitation)) * 0.35f;
            Humidity -= Mathf.Min(Precipitation, 1/Time.deltaTime) * 0.0001f * (Humidity - targetHumidity) * Time.deltaTime;
        } else {
            targetHumidity = 0.65f;
        }

        Humidity -= 0.0001f * (Humidity - .65f) * Time.deltaTime;
    }
}