using System.Collections;
using UnityEngine;


public class MainCameraController : MonoBehaviour
{
    Transform   _player;
    Vector3     _offset = new Vector3(-0.08f, 9.7f, -6.81f);
    float       _cameraSpeed = 2f;       


    /*
     * 게임 시작할 때 카메라를 움직이기 위해 GameReadyUI 구독
     */
    private void Awake()
    {
        _player = GameObject.Find("Chef").transform;
        GameReadyUI.CameraAction += MoveCamera;
    }


    /*
     * 프로젝트 종료시 구독 해제
     */
    private void OnDestroy()
    {
        GameReadyUI.CameraAction -= MoveCamera;
    }


    /*
     * Player의 위치에 따라 MainCamera의 위치를 변화시킴
     */
    void FixedUpdate()
    {
        if (_player.position.z < -1f)
            transform.position = Vector3.Lerp(transform.position, _offset + new Vector3(0, 0.8f, 0), Time.deltaTime * _cameraSpeed * 0.25f);
        else
            transform.position = Vector3.Lerp(transform.position, _offset, Time.deltaTime * _cameraSpeed * 0.25f);


        if(_player.position.x < -3f)
            transform.position = Vector3.Lerp(transform.position, _offset + new Vector3(-0.9f, 0, 0), Time.deltaTime * _cameraSpeed * 0.25f);
        else
            transform.position = Vector3.Lerp(transform.position, _offset, Time.deltaTime * _cameraSpeed * 0.25f);
    }


    /*
     * GameReadyUI에서 CameraAction이 Invoke될 때 호출되는 함수
     * -> 카메라 움직임을 재현한 코루틴을 호출
     */
    void MoveCamera()
    {
        StartCoroutine(StartMoving());
    }


    /*
     * 카메라 움직임을 재현한 코루틴
     * -> 시작 좌표, 목표 좌표, 지속 시간에 따라 while문이 실행됨
     */
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

        _offset.y = 9.7f;
    }
    
}
