using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    GameObject player;


    float offsetX = -0.08f;            // 카메라의 x좌표
    float offsetY = 9.7f;           // 카메라의 y좌표
    float offsetZ = -6.81f;          // 카메라의 z좌표

    float CameraSpeed = 2f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

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
