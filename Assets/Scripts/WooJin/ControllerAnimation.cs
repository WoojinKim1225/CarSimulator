using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[System.Serializable]
public struct ActionAnimationBinding {
    public InputActionReference reference;
    public string parameter;
}
public class ControllerAnimation : MonoBehaviour
{
    public InputActionReference button1, button2, button3, pinch, grab, joystick;
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Button1(InputAction.CallbackContext context) =>  _animator.SetFloat("Button 1", context.ReadValue<float>());
    
    void Button2(InputAction.CallbackContext context) => _animator.SetFloat("Button 2", context.ReadValue<float>());
    
    void Button3(InputAction.CallbackContext context) => _animator.SetFloat("Button 3", context.ReadValue<float>());

    void Pinch(InputAction.CallbackContext context) => _animator.SetFloat("Trigger", context.ReadValue<float>());

    void Grab(InputAction.CallbackContext context) => _animator.SetFloat("Grip", context.ReadValue<float>());

    void Joystick(InputAction.CallbackContext context) {
        _animator.SetFloat("Joy X", context.ReadValue<Vector2>().x);
        _animator.SetFloat("Joy Y", context.ReadValue<Vector2>().y);
    }

    void OnEnable()
    {
        if (button1 != null) {
            button1.action.started += Button1;
            button1.action.canceled += Button1;
        }
        if (button2 != null) {
            button2.action.started += Button2;
            button2.action.canceled += Button2;
        }
        if (button3 != null) {
            button3.action.started += Button3;
            button3.action.canceled += Button3;
        }
        if (pinch != null) {
            pinch.action.started += Pinch;
            pinch.action.canceled += Pinch;
            pinch.action.performed += Pinch;
        }
        if (grab != null) {
            grab.action.started += Grab;
            grab.action.canceled += Grab;
            grab.action.performed += Grab;
        }
        if (joystick != null) {
            joystick.action.started += Joystick;
            joystick.action.canceled += Joystick;
            joystick.action.performed += Joystick;
        }
    }
    

    void OnDisable()
    {
        if (button1 != null) {
            button1.action.started -= Button1;
            button1.action.canceled -= Button1;
        }
        if (button2 != null) {
            button2.action.started -= Button2;
            button2.action.canceled -= Button2;
        }
        if (button3 != null) {
            button3.action.started -= Button3;
            button3.action.canceled -= Button3;
        }
        if (pinch != null) {
            pinch.action.started -= Pinch;
            pinch.action.canceled -= Pinch;
            pinch.action.performed -= Pinch;
        }
        if (grab != null) {
            grab.action.started -= Grab;
            grab.action.canceled -= Grab;
            grab.action.performed -= Grab;
        }
        if (joystick != null) {
            joystick.action.started -= Joystick;
            joystick.action.canceled -= Joystick;
            joystick.action.performed -= Joystick;
        }
    }

}
