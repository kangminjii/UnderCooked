using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class StartSceneUI : MonoBehaviour, IPointerEnterHandler
{
    VanShutter _vanShutter;
    StartSceneCamera _startCamera;

    GameObject _startButton;
    GameObject _exitButton;
    GameObject _startText;

    bool _pressSpace = false;
    string _playScene = "[2]Minji";


    private void Start()
    {
        _vanShutter = FindObjectOfType<VanShutter>();
        _startCamera = FindObjectOfType<StartSceneCamera>();

        _startButton = transform.Find("StartButton").gameObject;
        _exitButton = transform.Find("ExitButton").gameObject;
        _startText = transform.Find("StartText").gameObject;


        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);
        

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


    public void OnClickNextScene()
    {
        SceneManager.LoadScene(_playScene);
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



    public void OnPointerEnter(PointerEventData eventData)
    {    
        Managers.Sound.Play("AudioClip/UI_Button_Drop", Define.Sound.Effect);
    }
}
