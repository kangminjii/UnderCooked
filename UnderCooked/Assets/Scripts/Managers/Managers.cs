using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;

    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();

    public static Managers Instance { get { Init(); return _instance; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    

    public bool IsGrab = false;

  
    static void Init()
    {
        //√ ±‚»≠
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            _instance = go.GetComponent<Managers>();
        }
    }

}
