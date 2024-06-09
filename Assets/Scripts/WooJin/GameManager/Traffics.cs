using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffics : MonoBehaviour
{
    public Material redLight, greenLight;
    public int state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        redLight.SetVector("_State", (state <= 0) ? Vector2.right : Vector2.zero);
        greenLight.SetVector("_State", (state > 0) ? Vector2.up : Vector2.zero);
    }
}
