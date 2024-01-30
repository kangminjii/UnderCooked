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
    

    static void Init()
    {
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

    //public bool IsGrab = false;
    //public bool IsPick_Prawn = false;
    //public bool IsCan_Pick = false;


    //public void SetIsPickPrawnTrue()
    //{
    //    Managers.Instance.IsPick_Prawn = true;
    //}

    //public void SetPrawnBool()
    //{
    //    Invoke("SetIsPickPrawnTrue", 0.5f);
    //}

    //public void CanPick()
    //{
    //    Managers.Instance.IsCan_Pick = true;
    //}

    //public void CanPickBool()
    //{
    //    Invoke("CanPick", 0.5f);
    //}

}
