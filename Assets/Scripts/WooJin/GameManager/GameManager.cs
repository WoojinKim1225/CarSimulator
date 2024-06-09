using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState {
        Clear = -1,
        Null = 0, 
        ReverseRun = 1, 
        SuddenAccel = 2,
    }
    public GameState gameState = GameState.Null;

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

    public Traffics traffics;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        traffics.state = (int)gameState;
        switch (gameState) {
            case GameState.ReverseRun:
            break;
            case GameState.SuddenAccel:
            break;
            case GameState.Clear:
            break;
            default:
            break;
        }
    }
}
