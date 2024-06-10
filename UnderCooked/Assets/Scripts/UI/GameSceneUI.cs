using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameSceneUI : MonoBehaviour
{
    Transform _player;
    float _cameraSpeed = 2f;
    Vector3 _offset = new Vector3(-0.08f, 9.7f, -6.81f);
    float _timeLimit = 100f;
    float _currentTime;
    bool _canUpdate = true;
    string _endScene = "[3]End";

    [SerializeField]
    Camera _camera;
    [Header("Image")]
    [SerializeField]
    GameObject _recipeImage;
    [SerializeField]
    GameObject _readyObject;
    [SerializeField]
    GameObject _startObject;
    [SerializeField]
    Image _spaceBar;
    [SerializeField]
    Image _startImage;
    [SerializeField]
    GameObject _endImage;
    [Header("Timer")]
    [SerializeField]
    Image _timerProgressBar;
    [SerializeField]
    Text _timeText;


    public static Action OrderAction;


    /*
    * 현재 시간 초기화
    * 
    */
    void Start()
    {
        _player = GameObject.Find("Chef").transform;
        _currentTime = _timeLimit;
        _timeText.text = FormatTime(_currentTime);

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
                _recipeImage.SetActive(false);
                _startImage.color = new Color(0, 0, 0, 0);
                
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

        _readyObject.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelReady_01", Define.Sound.Effect);

        yield return WaitForRealSeconds(2.5f);
        
        _readyObject.SetActive(false);
        _startObject.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelGo", Define.Sound.Effect);
        
        Time.timeScale = 1;

        OrderAction?.Invoke();

        yield return new WaitForSeconds(1.0f);

        _startObject.SetActive(false);
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


    // 타이머
    /*
     * Time이 흐를 때만 현재 시간을 변화시킴
     * -> 종료시 AppearEndingObject() 함수 호출
     * -> FormatTime에 맞춰 _timeText 변화
     * -> 시간에 맞춰 타이머바의 이미지 변화
     */
    void Update()
    {
        if (_canUpdate)
        {
            if (Time.timeScale > 0)
            {
                _currentTime -= Time.deltaTime;
            }

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _canUpdate = false;

                AppearEndingObject();
            }

            _timeText.text = FormatTime(_currentTime);
            _timerProgressBar.fillAmount = _currentTime / _timeLimit;
        }
    }

    /*
    * 시간값을 00:00 포멧에 맞춰 바꿔주는 함수
    */
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    /*
     * 게임 종료시 나오는 GameObject 함수
     * -> bgm 종료 및 이펙트 효과음 재생
     * -> 다음씬 로드
     */
    void AppearEndingObject()
    {
        _endImage.SetActive(true);

        Managers.Sound.Play("AudioClip/TimesUpSting", Define.Sound.Effect);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        StartCoroutine(LoadNextScene());
    }


    /*
     * 5초 뒤 다음 씬 로드
     */
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
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
