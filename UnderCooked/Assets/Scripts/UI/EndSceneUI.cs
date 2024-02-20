using System.Collections;
using System.Collections.Generic;
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
    string _playScene = "[2]Minji";


    void Start()
    {
        _successOrder = PlayerPrefs.GetInt("Success");
        _failOrder = PlayerPrefs.GetInt("Fail");
        Managers.Sound.Clear();
        Managers.Sound.Play("AudioClip/LevelVictorySound", Define.Sound.Effect);
        Invoke("PlaySound", 4f);
        //Managers.Sound.Play("AudioClip/RoundResults", Define.Sound.Bgm);
        ChangeText();
        TurnOnStar();
    }

    void TurnOnStar()
    {
        if(_totalScore >= 20)
        {
            //Managers.UI.FindDeepChild(transform, "Star1_Filled").gameObject.SetActive(true);
            StartCoroutine(ActivateStar
            (Managers.UI.FindDeepChild(transform, "Star1_Filled").gameObject,1f, "AudioClip/RoundResults_Star_01"));

        }
        if(_totalScore >= 60)
        {
            StartCoroutine(ActivateStar
            (Managers.UI.FindDeepChild(transform, "Star2_Filled").gameObject,2f, "AudioClip/RoundResults_Star_01"));
            //Managers.UI.FindDeepChild(transform, "Star2_Filled").gameObject.SetActive(true);
        }
        if (_totalScore >= 240)
        {
            StartCoroutine(ActivateStar
            (Managers.UI.FindDeepChild(transform, "Star3_Filled").gameObject, 3f, "AudioClip/RoundResults_Star_01"));
            //Managers.UI.FindDeepChild(transform, "Star3_Filled").gameObject.SetActive(true);
        }
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LoadStartScene();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            LoadPlayScene();
        }
    }

    void LoadStartScene()
    {
        SceneManager.LoadScene(_startScene);
    }

    void LoadPlayScene()
    {
        SceneManager.LoadScene(_playScene);
    }

    void PlaySound()
    {
        Managers.Sound.Play("AudioClip/RoundResults", Define.Sound.Bgm);
    }


    IEnumerator ActivateStar(GameObject star,float delay, string soundEffect)
    {
        yield return new WaitForSeconds(delay);
        Managers.Sound.Play(soundEffect, Define.Sound.Effect);
        star.SetActive(true);
    }
}
