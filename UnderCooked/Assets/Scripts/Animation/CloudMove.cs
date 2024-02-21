using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public GameObject cloudPrefab; // 이어붙일 구름 프리팹
    
    float speed = 1f; // 구름 이동 속도
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
        // 구름을 왼쪽으로 이동시킵니다.
        if (transform.position.x < -40)
            Destroy(this.gameObject);

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (nextCloud.transform.position.x < -8f)
        {
            // 현재 오브젝트의 오른쪽 끝에 구름을 생성합니다.
            Vector3 rightEndPosition = transform.TransformPoint(Vector3.right * 0.5f); // 오른쪽 끝 위치 계산
            nextCloud = Instantiate(cloudPrefab, rightEndPosition, Quaternion.identity);
            
        }

    }

}

