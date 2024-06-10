using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartAcc : MonoBehaviour
{
    void Awake()
    {
        transform.localPosition = new Vector3(0, 3, Random.Range(20, 50));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") Engine.Instance.isAccel = true;
    }

    void Update()
    {
        if (Engine.Instance.isAccel && Engine.Instance.carSpeed < 0.1f) GameManager.Instance.gameState = GameManager.GameState.Clear;
    }
}
