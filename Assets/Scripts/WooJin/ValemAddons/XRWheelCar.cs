using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class XRWheelCar : MonoBehaviour
{
    public XRKnob wheel;
    public CarControl carControl;
    public Transform wheelHand_L, wheelHand_R;
    public GameObject hand_L, hand_R;
    public SkinnedMeshRenderer oculusHandL, oculusHandR;

    void Awake()
    {
        wheel.selectEntered.AddListener(OnSelectHandShape);
        wheel.selectExited.AddListener(OnExitHand);
    }

    void Update()
    {
        carControl.steerUserInput = (wheel.value - 0.5f) * 2f;
    }

    public void OnSelectHandShape(SelectEnterEventArgs args) {
        if(args.interactorObject.transform.parent.gameObject.name == "Left Controller") {
            oculusHandL.enabled = false;
            Vector3 dir = args.interactorObject.transform.position - wheelHand_L.transform.position;
            Vector3 right = -Vector3.ProjectOnPlane(dir, wheelHand_L.transform.up);
            Vector3 forward = Vector3.Cross(right, wheelHand_L.transform.up);
            wheelHand_L.rotation = Quaternion.LookRotation(forward, wheelHand_L.transform.up);
            hand_L.SetActive(true);
        } else if (args.interactorObject.transform.parent.gameObject.name == "Right Controller") {
            oculusHandR.enabled = false;
            Vector3 dir = args.interactorObject.transform.position - wheelHand_R.transform.position;
            Vector3 right = Vector3.ProjectOnPlane(dir, wheelHand_R.transform.up);
            Vector3 forward = Vector3.Cross(right, wheelHand_R.transform.up);
            wheelHand_R.rotation = Quaternion.LookRotation(forward, wheelHand_R.transform.up);
            hand_R.SetActive(true);
        }
        
    }

    public void OnExitHand(SelectExitEventArgs args) {
        if(args.interactorObject.transform.parent.gameObject.name == "Left Controller") {
            oculusHandL.enabled = true;
            hand_L.SetActive(false);
        } else if (args.interactorObject.transform.parent.gameObject.name == "Right Controller") {
            oculusHandR.enabled = true;
            hand_R.SetActive(false);
        }
    }
}
