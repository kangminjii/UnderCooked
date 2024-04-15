using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    GameObject player;

    public delegate void MovingCamera();
    public event MovingCamera CameraAction;

    float offsetX = -0.08f;            // 카메라의 x좌표
    float offsetY = 9.7f;           // 카메라의 y좌표
    float offsetZ = -6.81f;          // 카메라의 z좌표

    float CameraSpeed = 2f;       // 카메라의 속도
    Vector3 TargetPos;            // 타겟의 위치

    private void Awake()
    {
        GameReadyUI.CameraAction += MoveCamera;
    }
    private void Start()
    {
        player = GameObject.Find("Chef").gameObject;
    }
    private void OnDestroy()
    {
        // CameraAction 이벤트에 대한 구독 해제
        GameReadyUI.CameraAction -= MoveCamera;
    }


    void FixedUpdate()
    {

        if (player.transform.position.z < -1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(offsetX, offsetY + 0.8f, offsetZ), Time.deltaTime * CameraSpeed * 0.25f);
        }
        else
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

    void MoveCamera()
    {
        StartCoroutine(StartMoving());
    }



    IEnumerator StartMoving()
    {
        float startY = transform.position.y;
        float targetY = 10.5f;
        float duration = 2.0f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(startY, targetY, t);

            Vector3 newPosition = transform.position;
            newPosition.y = newY;
            transform.position = newPosition;

            yield return null;
        }


        float reverseTargetY = 9.7f;
        float reverseDuration = 1.0f;

        float reverseElapsedTime = 0f;

        while (reverseElapsedTime < reverseDuration)
        {
            reverseElapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(reverseElapsedTime / reverseDuration);
            float newReverseY = Mathf.Lerp(targetY, reverseTargetY, t);

            Vector3 newReversePosition = transform.position;
            newReversePosition.y = newReverseY;
            transform.position = newReversePosition;

            yield return null;
        }

        offsetY = 9.7f;
    }
    
}
