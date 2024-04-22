using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VRCanvas : MonoBehaviour
{
    public TrackedPoseDriver source;

    public Vector3 sourceBeforePos;
    public Quaternion sourceBeforeRot;

    public Vector3 offset;
    
    private Canvas canvas;
    private RectTransform trs;
    [SerializeField] private float canvasRight, canvasUp;

    [SerializeField] private bool isHit;
    [SerializeField] private Vector3 hitPoint;
    [SerializeField] private Vector3 hitPointOS;

    [SerializeField] private bool isCanvasMoving;
    [SerializeField] private bool isSourceMoving;
    [SerializeField] private bool b;

    private WaitForEndOfFrame wait = new WaitForEndOfFrame();

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        trs = canvas.GetComponent<RectTransform>();
        isCanvasMoving = false;
    }

    private void Update() {
        isSourceMoving = Vector3.Magnitude(source.transform.position - sourceBeforePos) > 5f * Time.deltaTime || Quaternion.Angle(source.transform.rotation, sourceBeforeRot) > 10f * Time.deltaTime;

        if (!isHit && b && !isSourceMoving) {
            StartCoroutine(IMoveCanvas(source.transform.TransformPoint(offset), source.transform.rotation, Time.deltaTime));
            return;
        }

        sourceBeforePos = source.transform.position;
        sourceBeforeRot = source.transform.rotation;

        canvasRight = trs.rect.width / 2f;
        canvasUp = trs.rect.height / 2f;

        float dist = Vector3.Dot(source.transform.position - trs.position, trs.forward.normalized);
        if (Vector3.Dot(trs.forward, source.transform.forward) <= 0) {
            isHit = false;
            hitPoint = Vector3.zero;
            hitPointOS = Vector3.zero;
            //isCanvasMoving = true;
        } else {
            hitPoint = source.transform.position + dist * source.transform.forward / Vector3.Dot(trs.forward.normalized, -source.transform.forward);
            hitPointOS = trs.InverseTransformPoint(hitPoint);
            if (Mathf.Abs(hitPointOS.x) <= canvasRight && Mathf.Abs(hitPointOS.y) <= canvasUp && -dist <= 1f) {
                isHit = true;
            } else {
                isHit = false;
                //isCanvasMoving = true;
            }
        }

        

        b = isSourceMoving;
    }
    

    private IEnumerator IMoveCanvas(Vector3 targetPos, Quaternion targetRot, float deltaTime) {
        if (!isCanvasMoving) isCanvasMoving = true;
        else yield break;
        Debug.Log("a");
        while (trs.position != targetPos || trs.rotation != targetRot) {
            if (Vector3.Distance(trs.position, targetPos) < 0.01f && Quaternion.Angle(trs.rotation, targetRot) < 1f) {
                trs.position = targetPos;
                trs.rotation = targetRot;
                isCanvasMoving = false;
                isHit = true;
                yield break;
            } else {
                trs.position = Vector3.Lerp(targetPos, trs.position, Mathf.Exp(-deltaTime * 10f));
                trs.rotation = Quaternion.Slerp(targetRot, trs.rotation, Mathf.Exp(-deltaTime * 10f));
                yield return wait;
            }
        }
        isHit = true;
        isCanvasMoving = false;
        yield break;
        
    }
}
