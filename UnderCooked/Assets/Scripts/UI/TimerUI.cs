using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerUI : MonoBehaviour
{
    Image _timerProgressBar;
    Text _timeText;
    float _timeLimit = 150f;
    float _currentTime;
    bool _isReady;


    void Start()
    {
        _timeText = Managers.UI.FindDeepChild(transform, "Time").GetComponent<Text>();
        _timerProgressBar = Managers.UI.FindDeepChild(transform, "ProgressBar").GetComponent<Image>();
        _currentTime = _timeLimit;

        OrderUI.TimeStart += TimeStart;
    }


    void OnDestroy()
    {
        OrderUI.TimeStart -= TimeStart;
    }


    void Update()
    {
        // 종료 조건
        if (_currentTime <= 0)
            _currentTime = 0;

        // 시작 조건
        if(_isReady)
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

    void TimeStart(bool ready)
    {
        _isReady = ready;
    }
}
