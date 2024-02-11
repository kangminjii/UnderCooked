using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindDeepChild(child, name);

            if (result != null)
                return result;
        }

        return null;
    }
}
