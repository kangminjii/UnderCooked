using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class StartSceneUI : MonoBehaviour
{
    public Image StartImg;

    VanShutter _vanShutter;
    StartSceneCamera _startCamera;
    
    GameObject _startButton;
    GameObject _exitButton;
    GameObject _startText;

    float fadeDuration = 2.0f;
    bool _pressSpace = false;
    string _playScene = "[2]Game";


    private void Start()
    {
        _vanShutter = FindObjectOfType<VanShutter>();
        _startCamera = FindObjectOfType<StartSceneCamera>();

        _startButton = transform.Find("StartButton").gameObject;
        _exitButton = transform.Find("ExitButton").gameObject;
        _startText = transform.Find("StartText").gameObject;

        AudioSource bgmAudioSource = Managers.Sound.AudioSources[(int)Define.Sound.Bgm];
        if (bgmAudioSource.clip == null)
            Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        StartCoroutine(FadeOut());
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!_pressSpace)
            {
                StartCoroutine(_vanShutter.ShutterAnimation());
                StartCoroutine(_startCamera.CameraAnimation());

                Managers.Sound.Play("AudioClip/UI_PressStart", Define.Sound.Effect);
                Managers.Sound.BgmDown();

                _startButton.SetActive(true);
                _exitButton.SetActive(true);
                _startText.SetActive(false);

                _pressSpace = true;
            }
        }
    }


    IEnumerator FadeOut()
    {
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            StartImg.color = new Color(StartImg.color.r, StartImg.color.g, StartImg.color.b, alpha);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        StartImg.color = new Color(StartImg.color.r, StartImg.color.g, StartImg.color.b, endAlpha);

        Destroy(StartImg);
    }


    public void ClickSound()
    {
        Managers.Sound.Play("AudioClip/UI_Button_Drop", Define.Sound.Effect);
    }


    // ¾ÀÀüÈ¯
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
