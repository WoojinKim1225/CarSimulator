using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarFactory : MonoBehaviour
{
    public GameObject carPrefab;
    public Vector3 offset;
    public Quaternion rotationOffset;

    public float carSpeed;

    private bool b = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (b) return;
        b = true;
        GameObject obj = Instantiate(carPrefab, transform.position + offset, rotationOffset);
        obj.GetComponentInChildren<AiCar>().targetSpeed = carSpeed;
    }
}
