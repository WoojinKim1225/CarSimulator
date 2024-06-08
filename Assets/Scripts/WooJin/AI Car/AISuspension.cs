using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISuspension : MonoBehaviour
{
    public Rigidbody rb;
    public float k, c;
    public float distMax;
    public LayerMask whatIsGround;
    public float targetSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + transform.forward * 1f, -transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distMax, Color.green);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, distMax, whatIsGround)) {
            Vector3 vel = rb.GetPointVelocity(hit.point);
            float forceSize = k * (distMax - hit.distance) - c * Vector3.Dot(vel, ray.direction) / Time.fixedDeltaTime;
            float move = (targetSpeed - Vector3.Dot(vel, -transform.up)) / Time.fixedDeltaTime;
            rb.AddForceAtPosition(hit.normal * forceSize - Vector3.ProjectOnPlane(vel, ray.direction) / Time.fixedDeltaTime + move * (-transform.up), hit.point);
            Debug.DrawRay(hit.point, hit.normal * forceSize, Color.blue);
            Debug.DrawRay(hit.point, move * (-transform.up), Color.red);
        }
    }
}
