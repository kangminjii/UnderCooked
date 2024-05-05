using System;
using UnityEngine;
using UnityEngine.UI;


public class GameTimerUI : MonoBehaviour
{
    Image _timerProgressBar;
    Text _timeText;
    float _timeLimit = 100f;
    float _currentTime;
    bool _canUpdate = true;


    public static Action GameEnd;


    void Start()
    {
        _timeText = Managers.UI.FindDeepChild(transform, "Time").GetComponent<Text>();
        _timerProgressBar = Managers.UI.FindDeepChild(transform, "ProgressBar").GetComponent<Image>();
        _currentTime = _timeLimit;
    }


    void Update()
    {
        if (_canUpdate)
        {
            if (Time.timeScale > 0)
            {
                _currentTime -= Time.deltaTime;
            }

            // 종료 조건
            if (_currentTime <= 0)
            {
                _currentTime = 0;
                GameEnd.Invoke();
                _canUpdate = false;
            }

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
