using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance; // ���ϼ� ����

    InputManager _input = new InputManager();
    public static Managers Instance { get { Init(); return _instance; } }
    public static InputManager Input { get { return Instance._input; } }


  
    static void Init()
    {
        //�ʱ�ȭ
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
