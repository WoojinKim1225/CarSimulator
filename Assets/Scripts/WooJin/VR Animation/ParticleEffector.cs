using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffector : MonoBehaviour
{
    public InputActionReference reference;
    public ParticleSystem fx;

    private void Awake() {
        fx = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        reference.action.started += fxAction;
        reference.action.canceled += fxAction;
    }

    void OnDisable()
    {
        reference.action.started -= fxAction;
        reference.action.canceled -= fxAction;
    }

    private void fxAction(InputAction.CallbackContext context) {
        if (context.ReadValue<float>() > 0.5) {
            fx.Play();
        }
    }

}
