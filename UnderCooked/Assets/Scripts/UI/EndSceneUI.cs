using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndSceneUI : MonoBehaviour
{
    bool         _canSpace = false;
    [Header("GameScore")]
    int          _addingScore = 20;
    int          _minusScore = 10;
    int          _successScore;
    int          _failScore;
    int          _totalScore;
    int[]        _firstStageScore = new int[3]{ 20, 60, 240 };
    [Header("OrderCount")]
    int          _successOrder;
    int          _failOrder;
    [Header("LoadScene")]
    string       _startScene = "[1]Start";
    string       _playScene = "[2]Game";
    
    [SerializeField]
    Image        _image;
    [Header("Pass, Fail")]
    [SerializeField]
    Text[]       _order;
    [Header("Pass, Fail, Total")]
    [SerializeField]
    Text[]       _score;
    [Header("Star1, Star2, Star3")]
    [SerializeField]
    GameObject[] _star;


    /*
     * PlayerPrefs에서 성공, 실패한 주문 수를 받아옴
     * -> 성공, 실패, 합계 점수에 반영됨
     * 게임 종료 사운드 재생
     * 4초 이후에 "PlaySound" 함수를 실행함
     * 게임 결과를 보여주는 함수 실행
     */
    void Awake()
    {
        _successOrder = PlayerPrefs.GetInt("Success");
        _failOrder = PlayerPrefs.GetInt("Fail");

        _successScore = _successOrder * _addingScore;
        _failScore = _failOrder * _minusScore;
        _totalScore = _successScore - _failScore;
        if (_totalScore < 0)
            _totalScore = 0;

        Managers.Sound.Clear();
        Managers.Sound.Play("AudioClip/LevelVictorySound", Define.Sound.Effect);
        
        Invoke("PlaySound", 4f);
        ShowGameResult();
    }


    /*
     * 조건이 충족되면 키 입력에 따라 다른 씬으로 넘어감
     * EndScene의 뒷배경 이미지의 위치에 따라 위치값을 조절함
     */
    private void Update()
    {
        if (_canSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.SetString("SceneName", _startScene);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Loading");
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                PlayerPrefs.SetString("SceneName", _playScene);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Loading");
            }
        }

        if (_image.rectTransform.anchoredPosition.y > -1080)
        {
            _image.rectTransform.anchoredPosition -= new Vector2(0, 1);
        }
        else
        {
            _image.rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }


    /*
     * Bgm이 재생되는 함수
     */
    void PlaySound()
    {
        Managers.Sound.Play("AudioClip/RoundResults", Define.Sound.Bgm);
        _canSpace = true;
    }


    /*
     * 최종 결과를 보여주는 함수
     * -> 최종 점수에 따라 별이 활성화되는 애니메이션이 재생됨
     * -> 최종 결과표의 text UI를 변경함
     */
    void ShowGameResult()
    {
        if(_totalScore >= _firstStageScore[0])
        {
            StartCoroutine(ActivateStar(_star[0], 0.5f, "AudioClip/RoundResults_Star_01"));

            if (_totalScore >= _firstStageScore[1])
            {
                StartCoroutine(ActivateStar(_star[1], 1.5f, "AudioClip/RoundResults_Star_02"));

                if (_totalScore >= _firstStageScore[2])
                {
                    StartCoroutine(ActivateStar(_star[2], 2.5f, "AudioClip/RoundResults_Star_03"));
                }
            }
        }

        _order[0].text = _successOrder.ToString();
        _order[1].text = _failOrder.ToString();
        
        _score[0].text = _successScore.ToString();
        _score[1].text = _failScore.ToString();
        _score[2].text = _totalScore.ToString();
    }


    /*
     * Star를 활성화시키는 코루틴
     * -> delay 시간 이후에 star를 나타내며 이펙트 효과음이 재생됨
     */
    IEnumerator ActivateStar(GameObject star, float delay, string soundEffect)
    {
        yield return new WaitForSeconds(delay);
        
        star.SetActive(true);
        Managers.Sound.Play(soundEffect, Define.Sound.Effect);
    }

}
