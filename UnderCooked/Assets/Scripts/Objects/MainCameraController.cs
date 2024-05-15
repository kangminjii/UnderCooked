using System.Collections;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    GameObject  _player;
    Vector3     _offset = new Vector3(-0.08f, 9.7f, -6.81f);
    float       _cameraSpeed = 2f;       


    public delegate void MovingCamera();
    public event MovingCamera CameraAction;


    private void Start()
    {
        _player = GameObject.Find("Chef").gameObject;
        GameReadyUI.CameraAction += MoveCamera;
    }

    private void OnDestroy()
    {
        GameReadyUI.CameraAction -= MoveCamera;
    }


    void FixedUpdate()
    {
        if (_player.transform.position.z < -1f)
        {
            transform.position = Vector3.Lerp(transform.position, _offset + new Vector3(0, 0.8f, 0), Time.deltaTime * _cameraSpeed * 0.25f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _offset, Time.deltaTime * _cameraSpeed * 0.25f);
        }

        if(_player.transform.position.x < -3f)
        {
            transform.position = Vector3.Lerp(transform.position, _offset + new Vector3(-0.9f, 0, 0), Time.deltaTime * _cameraSpeed * 0.25f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _offset, Time.deltaTime * _cameraSpeed * 0.25f);
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

        _offset.y = 9.7f;
    }
    
}
