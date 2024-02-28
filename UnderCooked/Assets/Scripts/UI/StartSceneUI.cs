using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class StartSceneUI : MonoBehaviour
{
    VanShutter _vanShutter;
    StartSceneCamera _startCamera;
    
    public Image StartImg;
    GameObject _startButton;
    GameObject _exitButton;
    GameObject _startText;

    float fadeDuration = 2.0f;    // 색상 변화 지속 시간


    bool _pressSpace = false;
    string _playScene = "[2]Game";


    private void Start()
    {

        StartCoroutine(FadeOut());

        _vanShutter = FindObjectOfType<VanShutter>();
        _startCamera = FindObjectOfType<StartSceneCamera>();

        _startButton = transform.Find("StartButton").gameObject;
        _exitButton = transform.Find("ExitButton").gameObject;
        _startText = transform.Find("StartText").gameObject;   

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(_vanShutter.ShutterAnimation());
            StartCoroutine(_startCamera.CameraAnimation());

            if(!_pressSpace)
            {
                Managers.Sound.Play("AudioClip/UI_PressStart", Define.Sound.Effect);
               
                _pressSpace = true;
            }

            _startButton.SetActive(true);
            _exitButton.SetActive(true);
            _startText.SetActive(false);
        }
    }

    IEnumerator FadeOut()
    {
        // 시작 알파 값
        float startAlpha = 1.0f;
        // 목표 알파 값
        float endAlpha = 0.0f;

        // 경과 시간
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // 보간된 알파 값 계산
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // 이미지의 색상 업데이트 (알파 값만 변경)
            StartImg.color = new Color(StartImg.color.r, StartImg.color.g, StartImg.color.b, alpha);

            // 다음 프레임까지 대기
            yield return null;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
        }

        // 마지막으로 알파 값을 설정하여 완전히 투명하게 만듭니다.
        StartImg.color = new Color(StartImg.color.r, StartImg.color.g, StartImg.color.b, endAlpha);

        Destroy(StartImg);
    }


    public void ClickSound()
    {
        Managers.Sound.Play("AudioClip/UI_Button_Drop", Define.Sound.Effect);
    }


    public void OnClickNextScene()
    {
        PlayerPrefs.SetString("SceneName", _playScene);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Loading");

        Managers.Sound.Play("AudioClip/UI_Screen_In", Define.Sound.Effect);
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
