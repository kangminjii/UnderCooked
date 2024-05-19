using System.Collections;
using UnityEngine;


public class MainCameraController : MonoBehaviour
{
    Transform   _player;
    Vector3     _offset = new Vector3(-0.08f, 9.7f, -6.81f);
    float       _cameraSpeed = 2f;       


    /*
     * ���� ������ �� ī�޶� �����̱� ���� GameReadyUI ����
     */
    private void Awake()
    {
        _player = GameObject.Find("Chef").transform;
        GameReadyUI.CameraAction += MoveCamera;
    }


    /*
     * ������Ʈ ����� ���� ����
     */
    private void OnDestroy()
    {
        GameReadyUI.CameraAction -= MoveCamera;
    }


    /*
     * Player�� ��ġ�� ���� MainCamera�� ��ġ�� ��ȭ��Ŵ
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
     * GameReadyUI���� CameraAction�� Invoke�� �� ȣ��Ǵ� �Լ�
     * -> ī�޶� �������� ������ �ڷ�ƾ�� ȣ��
     */
    void MoveCamera()
    {
        StartCoroutine(StartMoving());
    }


    /*
     * ī�޶� �������� ������ �ڷ�ƾ
     * -> ���� ��ǥ, ��ǥ ��ǥ, ���� �ð��� ���� while���� �����
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
