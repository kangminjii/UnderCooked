using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameReadyUI : MonoBehaviour
{
    Transform _player;
    float _cameraSpeed = 2f;
    Vector3 _offset = new Vector3(-0.08f, 9.7f, -6.81f);
    [SerializeField]
    GameObject _recipe;
    [SerializeField]
    GameObject _ready;
    [SerializeField]
    GameObject  _start;
    [SerializeField]
    Image _spaceBar;
    [SerializeField]
    Image _image;
    [SerializeField]
    Camera _camera;


    public static Action OrderAction;



    void Start()
    {
        _player = GameObject.Find("Chef").transform;

        Managers.Sound.Play("AudioClip/TheNeonCity", Define.Sound.Bgm);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        StartCoroutine(SpaceBarCheck());
        
        Time.timeScale = 0;
        Cursor.visible = false;
    }
   

    IEnumerator SpaceBarCheck()
    {
        float startTime = Time.realtimeSinceStartup;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startTime = Time.realtimeSinceStartup;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _spaceBar.fillAmount = 0;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                _spaceBar.fillAmount = elapsedTime * 0.4f;
            }


            if (_spaceBar.fillAmount >= 1)
            {
                _recipe.SetActive(false);
                _image.color = new Color(0, 0, 0, 0);
                
                Managers.Sound.Play("AudioClip/Tutorial_Pop_Out", Define.Sound.Effect, 1f, 0.2f);
                StartCoroutine(ResumeGame());

                break;
            }

            yield return null;
        }
    }


    IEnumerator ResumeGame()
    {
        StartCoroutine(StartMoving());

        yield return WaitForRealSeconds(2f);

        _ready.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelReady_01", Define.Sound.Effect);

        yield return WaitForRealSeconds(2.5f);
        
        _ready.SetActive(false);
        _start.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelGo", Define.Sound.Effect);
        
        Time.timeScale = 1;

        OrderAction?.Invoke();

        yield return new WaitForSeconds(1.0f);

        _start.SetActive(false);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Play();
    }

    IEnumerator WaitForRealSeconds(float time)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return null;
        }
    }


    // 메인카메라
    /*
    * Player의 위치에 따라 MainCamera의 위치를 변화시킴
    */
    void FixedUpdate()
    {
        if (_player.position.z < -1f)
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _offset + new Vector3(0, 0.8f, 0), Time.deltaTime * _cameraSpeed * 0.25f);
        else
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _offset, Time.deltaTime * _cameraSpeed * 0.25f);


        if (_player.position.x < -3f)
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _offset + new Vector3(-0.9f, 0, 0), Time.deltaTime * _cameraSpeed * 0.25f);
        else
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _offset, Time.deltaTime * _cameraSpeed * 0.25f);
    }


    /*
     * 카메라 움직임을 재현한 코루틴
     * -> 시작 좌표, 목표 좌표, 지속 시간에 따라 while문이 실행됨
     */
    IEnumerator StartMoving()
    {
        float startY = _camera.transform.position.y;
        float targetY = 10.5f;
        float duration = 2.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(startY, targetY, t);

            Vector3 newPosition = _camera.transform.position;
            newPosition.y = newY;
            _camera.transform.position = newPosition;

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

            Vector3 newReversePosition = _camera.transform.position;
            newReversePosition.y = newReverseY;
            _camera.transform.position = newReversePosition;

            yield return null;
        }

        _offset.y = 9.7f;
    }


   
}
