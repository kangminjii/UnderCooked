using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameReadyUI : MonoBehaviour
{
    Image _spaceBar;
    GameObject _recipe;
    GameObject _ready;
    GameObject _start;
    

    public static Action OrderStart;
    public static Action CameraAction;


    void Start()
    {
        Managers.Sound.Play("AudioClip/TheNeonCity", Define.Sound.Bgm);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        _spaceBar = Managers.UI.FindDeepChild(transform, "SpaceBarCount").GetComponent<Image>();
        _recipe = Managers.UI.FindDeepChild(transform, "Recipe").gameObject;
        _ready = Managers.UI.FindDeepChild(transform, "Ready").gameObject;
        _start = Managers.UI.FindDeepChild(transform, "Start").gameObject;

        StartCoroutine(SpaceBarCheck());
        Time.timeScale = 0;
    }
   

    // 1. 스페이스바 게이지 채우기
    IEnumerator SpaceBarCheck()
    {
        float startTime = Time.realtimeSinceStartup;

        while (true)
        {
            // 스페이스바 조건에 따른 동작들
            if (Input.GetKeyDown(KeyCode.Space))
                startTime = Time.realtimeSinceStartup;
            else if (Input.GetKeyUp(KeyCode.Space))
                _spaceBar.fillAmount = 0;
            else if (Input.GetKey(KeyCode.Space))
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                _spaceBar.fillAmount = elapsedTime * 10f; // 0.4f
            }


            if (_spaceBar.fillAmount >= 1)
            {
                SetGameCondition();
                Managers.Sound.Play("AudioClip/Tutorial_Pop_Out", Define.Sound.Effect, 1f, 0.2f);
                break;
            }

            yield return null;
        }
    }

    // 2. 레디 문구 후 시작
    void SetGameCondition()
    {
        _recipe.SetActive(false);
        this.transform.GetComponent<Image>().color = new Color(0,0,0,0);

        StartCoroutine(ResumeGame());
    }


    // 3. 카메라 움직이기
    IEnumerator ResumeGame()
    {
        CameraAction.Invoke();

        yield return WaitForRealSeconds(2f);

        _ready.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelReady_01", Define.Sound.Effect);

        yield return WaitForRealSeconds(2.5f);
        
        _ready.SetActive(false);
        _start.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelGo", Define.Sound.Effect);
        
        Time.timeScale = 1;
        StartCoroutine(DisappearStartObject());
    }


    IEnumerator DisappearStartObject()
    {
        OrderStart.Invoke();
        
        yield return new WaitForSeconds(1.0f);   
        
        _start.SetActive(false);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Play();
    }


    IEnumerator WaitForRealSeconds(float time)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return null;
        }
    }
}
