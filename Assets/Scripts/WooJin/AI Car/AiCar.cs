

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiCar : MonoBehaviour
{
    private Rigidbody rb;
    public float k, c;
    public float distMax;
    public List<AISuspension> suspensions;
    public LayerMask whatIsGround;
    public float targetSpeed;
    public LineRenderer lineRenderer;

    [SerializeField] private int i = 1;
    // Start is called before the first frame update

    void Awake()
    {
        lineRenderer.positionCount = 8;
        for (int i = 0; i < 8; i++) {
            lineRenderer.SetPosition(i, new Vector3((int)Random.Range(0, 4) - 2.5f * 1.5f, 10f, i*20f + 10f));
        }    
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AISuspension suspension in suspensions) {
            suspension.rb = rb;
            suspension.k = k;
            suspension.c = c;
            suspension.distMax = distMax;
            suspension.whatIsGround = whatIsGround;
            suspension.targetSpeed = targetSpeed;
        }
    }

    void FixedUpdate()
    {
        int target = Mathf.Min(lineRenderer.positionCount-1, i+1);
        Debug.Log(i);
        if (i >= lineRenderer.positionCount) {
            Destroy(this.transform.parent.gameObject);
            return;
        }
        Vector3 pos = transform.parent.TransformPoint(lineRenderer.GetPosition(target));
        if (Vector3.Dot(transform.parent.TransformPoint(lineRenderer.GetPosition(i)) - rb.position, transform.forward) <= 0) i++;
        Vector3 dir = new Vector3(pos.x - transform.position.x, 0, pos.z - transform.position.z);
        suspensions[1].transform.rotation = Quaternion.LookRotation(dir, transform.up) * Quaternion.Euler(-90f, 0, 0);
        suspensions[2].transform.rotation = Quaternion.LookRotation(dir, transform.up) * Quaternion.Euler(-90f, 0, 0);
        Debug.DrawRay(transform.position, dir, Color.green);
    }


}
