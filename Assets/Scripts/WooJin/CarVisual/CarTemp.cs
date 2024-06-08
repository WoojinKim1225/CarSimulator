using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarTemp : MonoBehaviour
{
    [SerializeField] private Engine engine;
    [SerializeField] private float minAngle, maxAngle;
    [SerializeField] private float minValue, maxValue;
    private RectTransform rectTransform;
    [SerializeField] private Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float value = Mathf.Clamp(engine.currentTemperature, minValue, maxValue);
        float rot = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(minValue, maxValue, value));
        rectTransform.localRotation = Quaternion.Euler(0, 0, rot);
        text.text = Mathf.RoundToInt(value).ToString() + "\nºC";
    }
}
