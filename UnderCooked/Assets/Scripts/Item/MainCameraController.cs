using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    GameObject player;


    float offsetX = -0.08f;            // ī�޶��� x��ǥ
    float offsetY = 9.7f;           // ī�޶��� y��ǥ
    float offsetZ = -6.81f;          // ī�޶��� z��ǥ

    float CameraSpeed = 2f;       // ī�޶��� �ӵ�
    Vector3 TargetPos;                      // Ÿ���� ��ġ

    private void Start()
    {
        player = GameObject.Find("Chef").gameObject;
    }

    void FixedUpdate()
    {

        if (player.transform.position.z < -1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(offsetX, offsetY + 0.8f, offsetZ), Time.deltaTime * CameraSpeed * 0.25f);
        }
        else //(player.transform.position.z > -0.7f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(offsetX, offsetY, offsetZ), Time.deltaTime * CameraSpeed * 0.25f);
        }

        if(player.transform.position.x < -3f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(offsetX - 0.9f, offsetY, offsetZ), Time.deltaTime * CameraSpeed * 0.25f) ;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(offsetX , offsetY, offsetZ), Time.deltaTime * CameraSpeed * 0.25f);
        }

    }
    
}
