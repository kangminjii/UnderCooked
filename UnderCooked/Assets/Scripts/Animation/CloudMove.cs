using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public GameObject cloudPrefab; // �̾���� ���� ������
    
    float speed = 1f; // ���� �̵� �ӵ�
    GameObject nextCloud;
    Transform cloudTransform;


    void Start()
    {
        //cloudTransform.transform.position = this.transform.position;
        nextCloud = gameObject;
        cloudTransform = gameObject.transform;
        cloudTransform.position = new Vector3(51,22, 7);
    }

    void Update()
    {
        // ������ �������� �̵���ŵ�ϴ�.
        if (transform.position.x < -40)
            Destroy(this.gameObject);

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (nextCloud.transform.position.x < -8f)
        {
            // ���� ������Ʈ�� ������ ���� ������ �����մϴ�.
            Vector3 rightEndPosition = transform.TransformPoint(Vector3.right * 0.5f); // ������ �� ��ġ ���
            nextCloud = Instantiate(cloudPrefab, rightEndPosition, Quaternion.identity);
            
        }

    }

}

