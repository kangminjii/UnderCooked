using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObj : MonoBehaviour
{
    public float ScaleFactor = 0.001f;

    private void Update()
    {
        if(transform.parent.name == "BinSpawnPos")
        {
            float scale = Mathf.Lerp(1f, ScaleFactor, Time.time); // 1에서 0까지 선형 보간
            Vector3 newScale = new Vector3(scale, scale, scale );
            transform.localScale = newScale;
        }
    }

    // Animation 이벤트
    public void DeleteObject()
    {
        Destroy(this.gameObject);
    }
}
