using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartSceneUI : FadeInFadeOut
{
    Color       _startColor = new Color(0, 0, 0, 1);
    Color       _endColor = new Color(0, 0, 0, 0);
    bool        _pressSpace = false;
    string      _playScene = "[2]Game";
    Vector3     _cameraFinalPosition = new Vector3(-13f, -33.6f, -7f);
    Quaternion  _cameraFinalRotation = Quaternion.Euler(0, 7.83f, 0);
    Vector3     _cloudStartPosition = new Vector3(0f, -16f, 9.4f);

    [SerializeField]
    GameObject  _startButton;
    [SerializeField]
    GameObject  _exitButton;
    [SerializeField]
    GameObject  _startText;
    [SerializeField]
    Image       _image;
    [SerializeField]
    GameObject  _vanShutter;
    [SerializeField]
    Camera      _camera;
    [SerializeField]
    GameObject  _cloud;


    /*
     * ���۽� Ŀ�� ������ȭ
     * Bgm ���
     * FadeOut �ڷ�ƾ ���
     */
    private void Awake()
    {
        Cursor.visible = true;

        AudioSource bgmAudioSource = Managers.Sound.AudioSources[(int)Define.Sound.Bgm];
        if (bgmAudioSource.clip == null)
            Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        StartCoroutine(FadeOut(_startColor, _endColor, _image));
    }


    /*
     * SpaceŰ�� ���� �� ���� ���� ȭ������ �Ѿ�� ����
     * -> ī�޶� �� ���� �ִϸ��̼� ���
     * -> ��ư ���� ��� �� Bgm ���� ����
     * -> UI ǥ�� On/Off
     * -> Update���� �ٽ� ���� �ʵ��� bool������ ó��
     * 
     * ����ȭ�鿡 ����Ǵ� ���� Object�� ��ġ�� Update ó��
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_pressSpace)
            {
                StartCoroutine(CameraAnimation());
                StartCoroutine(ShutterAnimation());

                Managers.Sound.Play("AudioClip/UI_PressStart", Define.Sound.Effect);
                Managers.Sound.BgmDown();

                _startButton.SetActive(true);
                _exitButton.SetActive(true);
                _startText.SetActive(false);

                _pressSpace = true;
            }
        }

        _cloud.transform.Translate(Vector3.left * Time.deltaTime);
        
        if (_cloud.transform.position.x < -60)
            _cloud.transform.position = _cloudStartPosition;
    }


    /*
     * ���۽� ����Ǵ� ī�޶� �ִϸ��̼�
     * -> ī�޶��� position, rotation�� ��ǥ ���� ������ ������ �������Ѽ� ǥ��
     */
    IEnumerator CameraAnimation()
    {
        while (_camera.transform.position.x > _cameraFinalPosition.x || _camera.transform.position.y > _cameraFinalPosition.y || _camera.transform.position.z > _cameraFinalPosition.z)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _cameraFinalPosition, 3f * Time.deltaTime);
            _camera.transform.rotation = Quaternion.RotateTowards(_camera.transform.rotation, _cameraFinalRotation, 5f * Time.deltaTime);
            yield return null;
        }
    }


    /*
     * ���۽� ����Ǵ� ���� �ִϸ��̼�
     * -> ������ position�� ��ǥ ���� ������ ������ �������Ѽ� ǥ��
     */
    IEnumerator ShutterAnimation()
    {
        while (_vanShutter.transform.position.y < -20f)
        {
            _vanShutter.transform.position += new Vector3(0, 0.05f);
            yield return null;
        }
    }


    /*
     * ���۽� FadeOut�Ǵ� ������ ���ִ� �ڷ�ƾ
     */
    public override IEnumerator FadeOut(Color start, Color end, Image image)
    {
        yield return base.FadeOut(start, end, image);
        Destroy(_image);
    }


    /*
     * Trigger Event & Mouse Event
     * -> OnPointerSound     : Ŀ���� �ö� �� ���� ���
     * -> OnClickStage1Scene : ��������1 ��ư ���� ��, PlayerPrefs�� ���̸� ���� �� ���� �� �ε�
     * -> OnClickExit        : ���� ��ư ���� ��, ������ �� ���� ����
     */
    public void OnPointerSound()
    {
        Managers.Sound.Play("AudioClip/UI_Button_Drop", Define.Sound.Effect);
    }


    public void OnClickStage1Scene()
    {
        PlayerPrefs.SetString("SceneName", _playScene);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Loading");
    }


    public void OnClickExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
