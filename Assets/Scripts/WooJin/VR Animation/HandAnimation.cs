using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HandAnimation : MonoBehaviour
{
    public InputActionReference thumbRest, pinch, pinchTouch, grab;
    private Animator _animator;
    private float pinchAmount, grabAmount;
    private bool isPoint, isThumbRest;

    public Vector3 fingerCurlAmount;

    private readonly Vector3 baseAction = new Vector3(0, 0.3f, 0.3f);
    private readonly Vector3 pinchAction = new Vector3(0.5f, 0.6f, 0.3f);
    private readonly Vector3 grabAction = new Vector3(1f, 1f, 1f);

    void Awake()
    {
        _animator = GetComponent<Animator>();
        fingerCurlAmount = baseAction;
    }

    void Update() {
        pinchAmount = pinch.action.ReadValue<float>();
        grabAmount = grab.action.ReadValue<float>();
        isPoint = pinchTouch.action.ReadValue<float>() < 0.5f;
        isThumbRest = thumbRest.action.ReadValue<float>() > 0.5f;

        if (isThumbRest) fingerCurlAmount.x = Mathf.Max(0.5f, Mathf.Lerp(0f, 1f, grabAmount));
        else fingerCurlAmount.x = 0f;

        if (isPoint) fingerCurlAmount.y = 0;
        else fingerCurlAmount.y = Mathf.Max(Mathf.Lerp(0.2f, 0.6f, pinchAmount), Mathf.Lerp(0.2f, 1f, pinchAmount * grabAmount));

        fingerCurlAmount.z = Mathf.Lerp(0.2f, 1f, grabAmount);
        
        
        
        
        for (int i = 0; i < 3; i++)
            _animator.SetLayerWeight(i+1, fingerCurlAmount[i]);
    }
}
