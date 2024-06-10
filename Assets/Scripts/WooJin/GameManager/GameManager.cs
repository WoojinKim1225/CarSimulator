using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState {
        Clear = -1,
        Null = 0, 
        ReverseRun = 1, 
        SuddenAccel = 2,
    }
    public GameState gameState = GameState.Null;

    public GameObject RevMode, AccMode;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

    Traffics traffics;
    public bool b = false;

    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        traffics = Traffics.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        traffics.state = (int)gameState;
        switch (gameState) {
            case GameState.ReverseRun:
                if (!b) {
                    b = true;
                    Instantiate(RevMode, Vector3.zero, Quaternion.identity);
                }
            break;
            case GameState.SuddenAccel:
                if (!b) {
                    b = true;
                    Instantiate(AccMode, Vector3.zero, Quaternion.identity);
                }
            break;
            case GameState.Clear:
            break;
            default:
            break;
        }
    }

    public void GameOver()
    {
        gameState = GameState.Null;

    }
}
