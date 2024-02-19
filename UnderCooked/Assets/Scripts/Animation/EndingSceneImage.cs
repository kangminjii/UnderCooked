using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneImage : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y > -540)
            transform.position -= new Vector3(0, 1);
        else
            transform.position = new Vector3(960, 540);
    }
}
