using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanShutter : MonoBehaviour
{
    public IEnumerator ShutterAnimation()
    {
        while(transform.position.y < 7.9f)
        {
            transform.position += new Vector3(0,0.05f);
            yield return null;
        }

        transform.position = new Vector3(-0.77f, 20f, -2.61f);
    }
}
