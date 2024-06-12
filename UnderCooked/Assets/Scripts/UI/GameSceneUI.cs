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
    * ���� �ʱ�ȭ
    * -> Player ã��
    * -> ���� �ð� �ؽ�Ʈ �ʱ�ȭ
    * 
    * Bgm �ε� �� �Ͻ�����
    * CheckSpaceBar �ڷ�ƾ ����
    * timeScale 0 �� Ŀ�� ����ȭ
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
     * [Timer ����]
     * -> timeScale�� 1�� ��
     *  -> _currentTime�� ��ȭ
     *  -> Passing material ��ġ�� ��ȭ�Ͽ� �ִϸ��̼ǰ� ���� ���
     * -> _currentTime = 0�� ��(���������)
     *  -> _endImage Ȱ��ȭ
     *  -> bgm ���� �� ȿ���� ���
     *  -> LoadNextScene �ڷ�ƾ ����
     *  
     * -> �ð��� ���� _timeText�� _timerProgressBar ��ȭ
     *  -> 00:00 ���信 ���� �ؽ�Ʈ ����
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
     * [MainCamera ����]
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
     * ���� ���� �ڷ�ƾ
     * -> �����̽���Ű�� �������� spaceBar �������� ���� �ð���ŭ ����
     *  -> Ű�� ���� 0���� �ʱ�ȭ
     * -> �������� ��� ä������ �̹��� ��Ȱ��ȭ �� �����̹��� ����ȭ
     * -> ����Ʈ ��� �� ResumeGame �ڷ�ƾ ����
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
     * CheckSpaceBar ���� ������ ����Ǵ� �ڷ�ƾ
     * -> TimeScale�� 0�� ��
     *  -> MoveCamera �ڷ�ƾ ����
     *  -> 2�� �� readyObject Ȱ��ȭ �� ���� ���
     *  -> 2.5�� �� readyObject ��Ȱ��ȭ, startObject Ȱ��ȭ �� ���� ���
     * -> TimeScale�� 1�� ��
     *  -> OrderAction �̺�Ʈ �߻�
     *  -> 1�� �� startObject ��Ȱ��ȭ �� ���� ���
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
     * 5�� �� ���� �� �ε�
     */
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
    }


    /*
     * [MainCamera ����]
     * ī�޶� �������� ������ �ڷ�ƾ
     * -> ���� �ð�, ���� ��ǥ, ��ǥ ��ǥ�� ���� ī�޶��� ��ġ���� ����
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
