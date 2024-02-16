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

    GameObject _startButton;
    GameObject _exitButton;
    Text _startText;
    Text _selectedText;
    Text _originalText;

    string _playScene = "[2]Minji";

    private void Start()
    {
        _vanShutter = FindObjectOfType<VanShutter>();
        _startCamera = FindObjectOfType<StartSceneCamera>();

        _startButton = transform.Find("StartButton").gameObject;
        _exitButton = transform.Find("ExitButton").gameObject;
        _startText = transform.Find("StartText").GetComponent<Text>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(_vanShutter.ShutterAnimation());
            StartCoroutine(_startCamera.CameraAnimation());

            _startButton.SetActive(true);
            _exitButton.SetActive(true);
            _startText.text = "";
        }
    }


    public void OnClickNextScene()
    {
        SceneManager.LoadScene(_playScene);
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
