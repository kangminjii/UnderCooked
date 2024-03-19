using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Material Material;  // 이동 효과를 적용할 머터리얼
    float _speed = -1f;

    void Update()
    {
        float offsetX = Time.time * _speed;
        Material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }
}
