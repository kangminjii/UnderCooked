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
    * ���� �ð� �ʱ�ȭ
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


    // Ÿ�̸�
    /*
     * Time�� �带 ���� ���� �ð��� ��ȭ��Ŵ
     * -> ����� AppearEndingObject() �Լ� ȣ��
     * -> FormatTime�� ���� _timeText ��ȭ
     * -> �ð��� ���� Ÿ�̸ӹ��� �̹��� ��ȭ
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
    * �ð����� 00:00 ���信 ���� �ٲ��ִ� �Լ�
    */
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    /*
     * ���� ����� ������ GameObject �Լ�
     * -> bgm ���� �� ����Ʈ ȿ���� ���
     * -> ������ �ε�
     */
    void AppearEndingObject()
    {
        _endImage.SetActive(true);

        Managers.Sound.Play("AudioClip/TimesUpSting", Define.Sound.Effect);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        StartCoroutine(LoadNextScene());
    }


    /*
     * 5�� �� ���� �� �ε�
     */
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
    }


    // ����ī�޶�
    /*
    * Player�� ��ġ�� ���� MainCamera�� ��ġ�� ��ȭ��Ŵ
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
     * ī�޶� �������� ������ �ڷ�ƾ
     * -> ���� ��ǥ, ��ǥ ��ǥ, ���� �ð��� ���� while���� �����
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
