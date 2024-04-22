using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;

    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SoundManager _sound = new SoundManager();


    public static Managers Instance { get { Init(); return _instance; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    

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
            _instance._sound.Init();
        }
    }

}
