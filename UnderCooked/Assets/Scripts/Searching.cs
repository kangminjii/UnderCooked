using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Decorator Pattern
// ��ü�� �߰����� ����� �������� ÷���ϴ� ��
// ���� Ŭ������ ����� ���� ���� ����� �����ϰ� Ȯ���Ѵ�.
// > ���� ������ ���� ������ �ʴ´�.

// ������Ʈ ��� ��Ű��ó �Ǵ� �������̽� Ȱ���� �ַ� ��




public class Searching : MonoBehaviour
{

    private Material commonMaterial;
    private Material instanceMaterial;

    private void Start()
    {
        commonMaterial = GetComponent<MeshRenderer>().material;
        instanceMaterial = Instantiate(commonMaterial);
        GetComponent<MeshRenderer>().material = instanceMaterial;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            instance
        }
    }



}
