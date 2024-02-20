using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Material material;  // 이동 효과를 적용할 머터리얼
    float _speed = -1f;

    void Update()
    {
        // 이동 방향에 따라 텍스처 스크롤
        float offsetX = Time.time * _speed;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }
}
