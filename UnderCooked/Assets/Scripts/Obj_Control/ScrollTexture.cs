using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Material material;  // 이동 효과를 적용할 머터리얼
    public Vector2 scrollSpeed = new Vector2(-1f, 0f);  // 스크롤 속도

    void Update()
    {
        // 이동 방향에 따라 텍스처 스크롤
        float offsetX = Time.time * scrollSpeed.x;
        float offsetY = Time.time * scrollSpeed.y;

        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
