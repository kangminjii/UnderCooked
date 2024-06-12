using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameSceneUI : MonoBehaviour
{
    Transform   _player;
    Vector3     _offset = new Vector3(-0.08f, 9.7f, -6.81f);
    float       _cameraSpeed = 2f;
    float       _timeLimit = 100f;
    float       _currentTime;
    bool        _canUpdate = true;
    string      _endScene = "[3]End";

    [Header("Image")]
    [SerializeField]
    GameObject  _recipeImage;
    [SerializeField]
    GameObject  _readyObject;
    [SerializeField]
    GameObject  _startObject;
    [SerializeField]
    Image       _spaceBar;
    [SerializeField]
    Image       _startImage;
    [SerializeField]
    GameObject  _endImage;
    [Header("Timer")]
    [SerializeField]
    Image       _timerProgressBar;
    [SerializeField]
    Text        _timeText;
    [Header("Objects")]
    [SerializeField]
    Camera       _camera;
    [SerializeField]
    MeshRenderer _passing;


    public static Action OrderAction;


    /*
    * 변수 초기화
    * -> Player 찾기
    * -> 현재 시간 텍스트 초기화
    * 
    * Bgm 로드 후 일시정지
    * CheckSpaceBar 코루틴 실행
    * timeScale 0 및 커서 투명화
    */
    void Awake()
    {
        _player = GameObject.Find("Chef").transform;
        
        _currentTime = _timeLimit;
        _timeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(_currentTime / 60f), Mathf.FloorToInt(_currentTime % 60f));

        Managers.Sound.Play("AudioClip/TheNeonCity", Define.Sound.Bgm);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        StartCoroutine(CheckSpaceBar());
        
        Time.timeScale = 0;
        Cursor.visible = false;
    }


    /*
     * [Timer 관련]
     * -> timeScale이 1일 때
     *  -> _currentTime을 변화
     *  -> Passing material 위치를 변화하여 애니메이션과 같이 출력
     * -> _currentTime = 0일 때(게임종료시)
     *  -> _endImage 활성화
     *  -> bgm 종료 및 효과음 재생
     *  -> LoadNextScene 코루틴 실행
     *  
     * -> 시간에 맞춰 _timeText와 _timerProgressBar 변화
     *  -> 00:00 포멧에 맞춰 텍스트 변경
     */
    void Update()
    {
        if (_canUpdate)
        {
            if (Time.timeScale > 0)
            {
                _currentTime -= Time.deltaTime;

                float offsetX = Time.time * -1f;
                _passing.material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
            }

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _canUpdate = false;

                _endImage.SetActive(true);

                Managers.Sound.Play("AudioClip/TimesUpSting", Define.Sound.Effect);
                Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

                StartCoroutine(LoadNextScene());
            }

            _timeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(_currentTime / 60f), Mathf.FloorToInt(_currentTime % 60f));
            _timerProgressBar.fillAmount = _currentTime / _timeLimit;
        }
    }


    /*
     * [MainCamera 관련]
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
     * 시작 조건 코루틴
     * -> 스페이스바키가 눌러져야 spaceBar 게이지를 누른 시간만큼 증가
     *  -> 키를 떼면 0으로 초기화
     * -> 게이지가 모두 채워지면 이미지 비활성화 및 시작이미지 투명화
     * -> 이펙트 재생 후 ResumeGame 코루틴 실행
     */
    IEnumerator CheckSpaceBar()
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


    /*
     * CheckSpaceBar 조건 충족시 실행되는 코루틴
     * -> TimeScale이 0일 때
     *  -> MoveCamera 코루틴 실행
     *  -> 2초 후 readyObject 활성화 및 사운드 재생
     *  -> 2.5초 후 readyObject 비활성화, startObject 활성화 및 사운드 재생
     * -> TimeScale이 1일 때
     *  -> OrderAction 이벤트 발생
     *  -> 1초 후 startObject 비활성화 및 사운드 재생
     */
    IEnumerator ResumeGame()
    {
        StartCoroutine(MoveCamera());

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < 2f)
        {
            yield return null;
        }

        _readyObject.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelReady_01", Define.Sound.Effect);

        startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < 2.5f)
        {
            yield return null;
        }

        _readyObject.SetActive(false);
        _startObject.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelGo", Define.Sound.Effect);
        
        Time.timeScale = 1;
        OrderAction?.Invoke();

        yield return new WaitForSeconds(1.0f);

        _startObject.SetActive(false);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Play();
    }


    /*
     * 5초 뒤 다음 씬 로드
     */
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
    }


    /*
     * [MainCamera 관련]
     * 카메라 움직임을 재현한 코루틴
     * -> 지속 시간, 시작 좌표, 목표 좌표에 따라 카메라의 위치값을 조절
     */
    IEnumerator MoveCamera()
    {
        yield return ChangeCameraPosition(0f, 2.0f, _camera.transform.position.y, 10.5f);
        yield return ChangeCameraPosition(0f, 1.0f, 10.5f, 9.7f);
       
        _offset.y = 9.7f;
    }


    IEnumerator ChangeCameraPosition(float time, float duration, float startY, float targetY)
    {
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / duration);
            float newY = Mathf.Lerp(startY, targetY, t);

            Vector3 newPosition = _camera.transform.position;
            newPosition.y = newY;
            _camera.transform.position = newPosition;

            yield return null;
        }
    }
   
}
