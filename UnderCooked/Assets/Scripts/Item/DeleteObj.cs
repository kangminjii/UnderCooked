using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObj : MonoBehaviour
{
    public float scaleFactor = 0.001f;
    private void Update()
    {
        if(transform.parent.name == "BinSpawnPos")
        {
            float scale = Mathf.Lerp(1f, scaleFactor, Time.time); // 1에서 0까지 선형 보간
            Vector3 newScale = new Vector3(scale, scale, scale );
            transform.localScale = newScale;
        }
    }
    public void DeletObj()
    {
        Destroy(this.gameObject);
    }
}
