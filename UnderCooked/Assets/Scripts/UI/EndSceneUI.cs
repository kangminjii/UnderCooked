using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndSceneUI : MonoBehaviour
{
    int _addingScore = 20;
    int _minusScore = 10;
    int _totalScore;
    int _successOrder;
    int _failOrder;

    string _startScene = "[1]Start";
    string _playScene = "[2]Game";

    bool _canSpace = false;


    void Start()
    {
        _successOrder = PlayerPrefs.GetInt("Success");
        _failOrder = PlayerPrefs.GetInt("Fail");
        
        Managers.Sound.Clear();
        Managers.Sound.Play("AudioClip/LevelVictorySound", Define.Sound.Effect);
        Invoke("PlaySound", 4f);
        
        ChangeText();
        TurnOnStar();
    }

    private void Update()
    {
        if (_canSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadScene(_startScene);
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                LoadScene(_playScene);
            }
        }
    }


    void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("SceneName", sceneName);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Loading");
    }


    void PlaySound()
    {
        Managers.Sound.Play("AudioClip/RoundResults", Define.Sound.Bgm);
        _canSpace = true;
    }


    void TurnOnStar()
    {
        if(_totalScore >= 20)
        {
            GameObject star = Managers.UI.FindDeepChild(transform, "Star1_Filled").gameObject;
            StartCoroutine(ActivateStar(star, 0.5f, "AudioClip/RoundResults_Star_01"));
        }
        if(_totalScore >= 60)
        {
            GameObject star = Managers.UI.FindDeepChild(transform, "Star2_Filled").gameObject;
            StartCoroutine(ActivateStar(star,1.5f, "AudioClip/RoundResults_Star_02"));
        }
        if (_totalScore >= 240)
        {
            GameObject star = Managers.UI.FindDeepChild(transform, "Star3_Filled").gameObject;
            StartCoroutine(ActivateStar(star, 2.5f, "AudioClip/RoundResults_Star_03"));
        }
    }


    IEnumerator ActivateStar(GameObject star, float delay, string soundEffect)
    {
        yield return new WaitForSeconds(delay);
        
        star.SetActive(true);
        Managers.Sound.Play(soundEffect, Define.Sound.Effect);
    }


    void ChangeText()
    {
        Managers.UI.FindDeepChild(transform, "PassCount").GetComponent<Text>().text = _successOrder.ToString();
        Managers.UI.FindDeepChild(transform, "FailCount").GetComponent<Text>().text = _failOrder.ToString();

        int successScore = _successOrder * _addingScore;
        int failScore = _failOrder * _minusScore;
        _totalScore = successScore - failScore;

        if (_totalScore < 0)
            _totalScore = 0;

        Managers.UI.FindDeepChild(transform, "PassScore").GetComponent<Text>().text = successScore.ToString();
        Managers.UI.FindDeepChild(transform, "FailScore").GetComponent<Text>().text = failScore.ToString();
        Managers.UI.FindDeepChild(transform, "TotalScore").GetComponent<Text>().text = _totalScore.ToString();
    }

}
