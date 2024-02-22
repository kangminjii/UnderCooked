using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Material material;  // �̵� ȿ���� ������ ���͸���
    float _speed = -1f;

    void Update()
    {
        // �̵� ���⿡ ���� �ؽ�ó ��ũ��
        float offsetX = Time.time * _speed;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }
}
