using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class TimerUI : MonoBehaviour
{
    Image _timerProgressBar;
    Text _timeText;
    float _timeLimit = 20f;
    float _currentTime;

    string _endScene = "[3]Ending";

    void Start()
    {
        _timeText = Managers.UI.FindDeepChild(transform, "Time").GetComponent<Text>();
        _timerProgressBar = Managers.UI.FindDeepChild(transform, "ProgressBar").GetComponent<Image>();
        _currentTime = _timeLimit;
    }


    void Update()
    {
        // 종료 조건
        if (_currentTime <= 0)
        {
            _currentTime = 0;
            SceneManager.LoadScene(_endScene);
        }

        if(Time.timeScale > 0)
        {
            _currentTime -= Time.deltaTime;
            _timeText.text = FormatTime(_currentTime);
            _timerProgressBar.fillAmount = _currentTime / _timeLimit;
        }
      
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
