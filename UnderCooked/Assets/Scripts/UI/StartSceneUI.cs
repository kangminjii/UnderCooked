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
     * 시작시 커서 불투명화
     * Bgm 재생
     * FadeOut 코루틴 재생
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
     * Space키를 누를 때 다음 진행 화면으로 넘어가는 조건
     * -> 카메라 및 셔터 애니메이션 재생
     * -> 버튼 사운드 재생 및 Bgm 사운드 감소
     * -> UI 표시 On/Off
     * -> Update문에 다시 오지 않도록 bool값으로 처리
     * 
     * 시작화면에 재생되는 구름 Object의 위치를 Update 처리
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
     * 시작시 재생되는 카메라 애니메이션
     * -> 카메라의 position, rotation을 목표 값에 도달할 때까지 증가시켜서 표현
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
     * 시작시 재생되는 셔터 애니메이션
     * -> 셔터의 position을 목표 값에 도달할 때까지 증가시켜서 표현
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
     * 시작시 FadeOut되는 연출을 해주는 코루틴
     */
    public override IEnumerator FadeOut(Color start, Color end, Image image)
    {
        yield return base.FadeOut(start, end, image);
        Destroy(_image);
    }


    /*
     * Trigger Event & Mouse Event
     * -> OnPointerSound     : 커서가 올라갈 때 사운드 재생
     * -> OnClickStage1Scene : 스테이지1 버튼 누를 때, PlayerPrefs에 씬이름 저장 및 다음 씬 로드
     * -> OnClickExit        : 종료 버튼 누를 때, 에디터 및 어플 종료
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
