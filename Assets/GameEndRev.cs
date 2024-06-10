using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndRev : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") GameManager.Instance.gameState = GameManager.GameState.Clear;
    }
}
