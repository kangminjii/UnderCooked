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
            float scale = Mathf.Lerp(1f, ScaleFactor, Time.time); // 1���� 0���� ���� ����
            Vector3 newScale = new Vector3(scale, scale, scale );
            transform.localScale = newScale;
        }
    }

    // Animation �̺�Ʈ
    public void DeleteObject()
    {
        Destroy(this.gameObject);
    }
}
