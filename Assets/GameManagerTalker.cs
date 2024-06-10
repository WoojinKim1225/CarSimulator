using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTalker : MonoBehaviour
{
    public GameManager gameManager;
    public string sceneName;
    public GameObject start, rev, acc;

    public static GameManagerTalker instance;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        instance = this;
        start.SetActive(true);
    }

    public void startRev() {
        gameManager.gameState = GameManager.GameState.ReverseRun;
        start.SetActive(false);
    }

    public void startAcc() {
        gameManager.gameState = GameManager.GameState.SuddenAccel;
        start.SetActive(false);
    }

    public void gameOver() {
        if (gameManager.gameState == GameManager.GameState.ReverseRun) 
        {
            rev.SetActive(true);
        }else if (gameManager.gameState == GameManager.GameState.SuddenAccel) {
            acc.SetActive(true);
        }
        gameManager.GameOver();
    }
}
