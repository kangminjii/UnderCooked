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
            Debug.Log("TriggeredEnter");
        
            Color newEmissionColor = new Color(0.5f, 0.45f, 0.4f, 0f);
            instanceMaterial.SetColor("_EmissionColor", newEmissionColor);
            instanceMaterial.EnableKeyword("_EMISSION");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("TriggeredExit");

            instanceMaterial.DisableKeyword("_EMISSION");

        }
    }


}
