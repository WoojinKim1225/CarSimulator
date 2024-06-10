using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffics : MonoBehaviour
{
    public Material redLight, greenLight;
    public int state;

    private static Traffics _instance;
    public static Traffics Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Traffics>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Traffics>();
                    singletonObject.name = typeof(Traffics).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        redLight.SetVector("_State", (state <= 0) ? Vector2.right : Vector2.zero);
        greenLight.SetVector("_State", (state > 0) ? Vector2.up : Vector2.zero);
    }
}
