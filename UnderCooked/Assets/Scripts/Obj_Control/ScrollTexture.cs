using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Material material;  // �̵� ȿ���� ������ ���͸���
    public Vector2 scrollSpeed = new Vector2(-1f, 0f);  // ��ũ�� �ӵ�

    void Update()
    {
        // �̵� ���⿡ ���� �ؽ�ó ��ũ��
        float offsetX = Time.time * scrollSpeed.x;
        float offsetY = Time.time * scrollSpeed.y;

        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
