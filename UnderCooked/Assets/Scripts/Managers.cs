using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance; // 유일성 보장

    InputManagers _input = new InputManagers();
    public static Managers Instance { get { Init(); return _instance; } }
    public static InputManagers Input { get { return Instance._input; } }


    void Update()
    {
        _input.OnUpdate();
    }

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
