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

    float fadeDuration = 2.0f;    // ���� ��ȭ ���� �ð�


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
        // ���� ���� ��
        float startAlpha = 1.0f;
        // ��ǥ ���� ��
        float endAlpha = 0.0f;

        // ��� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // ������ ���� �� ���
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // �̹����� ���� ������Ʈ (���� ���� ����)
            StartImg.color = new Color(StartImg.color.r, StartImg.color.g, StartImg.color.b, alpha);

            // ���� �����ӱ��� ���
            yield return null;

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;
        }

        // ���������� ���� ���� �����Ͽ� ������ �����ϰ� ����ϴ�.
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
