using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrawnScripts : MonoBehaviour
{
    void Update()
    {
        if(!Managers.Instance.IsPick_Prawn)
        {
            Destroy(this.gameObject);
        }
    }
}
