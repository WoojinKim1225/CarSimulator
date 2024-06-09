using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GearChanger : MonoBehaviour
{
    public InputActionReference gearReference;
    private bool b;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float f = gearReference.action.ReadValue<Vector2>().y;
        if (AutomaticTransmission.gear == EGear.Manual) {
            if (f > 0.5f) {
                if (AutomaticTransmission.clutchNumber < 5 && !b) {
                    AutomaticTransmission.clutchNumber++;
                    b = true;
                }
            }
            else if (f < -0.5f) {
                if (AutomaticTransmission.clutchNumber > 1 && !b) {
                    AutomaticTransmission.clutchNumber--;
                    b = true;
                }
            }
            else if (Mathf.Abs(f) < 0.2f) {
                b = false;
            }
        }
    }
}
