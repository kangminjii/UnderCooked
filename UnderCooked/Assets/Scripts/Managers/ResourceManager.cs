using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    //public T Load<T>(string path) where T : Object
    //{
    //    if (typeof(T) == typeof(GameObject))
    //    {
    //        string name = path;
    //        int index = name.LastIndexOf('/');
    //        if (index >= 0)
    //            name = name.Substring(index + 1);

    //        //GameObject go = Managers.Pool.GetOriginal(name);
    //        GameObject go = Resources.Load(name) as GameObject;
    //        if (go != null)
    //            return go as T;
    //    }

    //    return Resources.Load<T>(path);
    //}

    public GameObject Instantiate(string path, Vector3? pos = null, Quaternion? rot = null, Transform parent = null)
    {
        GameObject original = Resources.Load<GameObject>($"Prefabs/{path}");
        pos = pos ?? Vector3.zero;
        rot = rot ?? Quaternion.identity;

        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(original, (Vector3)pos, (Quaternion)rot, parent);
    }

 
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
