using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성 보장

    InputManagers _input = new InputManagers();
    public static Managers Instance { get { Init(); return s_instance; } }
    public static InputManagers Input { get { return Instance._input; } }
    void Start()
    {

    }


    void Update()
    {

    }

    static void Init()
    {
        //초기화

        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }


            DontDestroyOnLoad(go);

            s_instance = go.GetComponent<Managers>();
        }
    }
}
