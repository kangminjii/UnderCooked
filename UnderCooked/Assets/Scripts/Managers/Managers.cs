using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance; // 유일성 보장
    public static Managers Instance { get { Init(); return _instance; } }


    InputManager _input = new InputManager();

    PoolManager _pool = new PoolManager();
    public static InputManager Input { get { return Instance._input; } }

    public static PoolManager Pool { get { return Instance._pool; } }

    


  
    static void Init()
    {
        //초기화
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
