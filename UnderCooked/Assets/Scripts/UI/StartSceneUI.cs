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
    Vector3     _finalPosition = new Vector3(-1.57f, 4.17f, -9.05f);
    Quaternion  _finalRotation = Quaternion.Euler(0, 7.83f, 0);
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
    }


    /*
     * 시작시 재생되는 카메라 애니메이션
     * -> 카메라의 position, rotation을 목표 값에 도달할 때까지 증가시켜서 표현
     */
    IEnumerator CameraAnimation()
    {
        while (_camera.transform.position.x > _finalPosition.x || _camera.transform.position.y > _finalPosition.y || _camera.transform.position.z > _finalPosition.z)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _finalPosition, 3f * Time.deltaTime);
            _camera.transform.rotation = Quaternion.RotateTowards(_camera.transform.rotation, _finalRotation, 5f * Time.deltaTime);

            yield return null;
        }
    }


    /*
     * 시작시 재생되는 셔터 애니메이션
     * -> 셔터의 position을 목표 값에 도달할 때까지 증가시켜서 표현
     */
    IEnumerator ShutterAnimation()
    {
        while (_vanShutter.transform.position.y < 7.9f)
        {
            _vanShutter.transform.position += new Vector3(0, 0.05f);
            yield return null;
        }

        _vanShutter.transform.position = new Vector3(-0.77f, 20f, -2.61f);
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
