using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    public new CanvasRenderer renderer;

    void Update()
    {
        Debug.Log(renderer.cull);
    }
}