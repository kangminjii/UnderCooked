using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameSceneTimerUI : MonoBehaviour
{
    float       _timeLimit = 100f;
    float       _currentTime;
    bool        _canUpdate = true;
    string      _endScene = "[3]End";
    [SerializeField]
    Image       _timerProgressBar;
    [SerializeField]
    Text        _timeText;
    [SerializeField]
    GameObject  _endImage;
    

    /*
     * 현재 시간 초기화
     */
    void Awake()
    {
        _currentTime = _timeLimit;
        _timeText.text = FormatTime(_currentTime);
    }


    /*
     * Time이 흐를 때만 현재 시간을 변화시킴
     * -> 종료시 AppearEndingObject() 함수 호출
     * -> FormatTime에 맞춰 _timeText 변화
     * -> 시간에 맞춰 타이머바의 이미지 변화
     */
    void Update()
    {
        if (_canUpdate)
        {
            if (Time.timeScale > 0)
            {
                _currentTime -= Time.deltaTime;
            }

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _canUpdate = false;

                AppearEndingObject();
            }

            _timeText.text = FormatTime(_currentTime);
            _timerProgressBar.fillAmount = _currentTime / _timeLimit;
        }
    }


    /*
     * 시간값을 00:00 포멧에 맞춰 바꿔주는 함수
     */
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    /*
     * 게임 종료시 나오는 GameObject 함수
     * -> bgm 종료 및 이펙트 효과음 재생
     * -> 다음씬 로드
     */
    void AppearEndingObject()
    {
        _endImage.SetActive(true);
        
        Managers.Sound.Play("AudioClip/TimesUpSting", Define.Sound.Effect);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();
        
        StartCoroutine(LoadNextScene());
    }


    /*
     * 5초 뒤 다음 씬 로드
     */
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
    }

}
