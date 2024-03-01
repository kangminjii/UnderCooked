using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameReadyUI : MonoBehaviour
{
    Image _spaceBar;

    public static Action OrderStart;
    public static Action CameraAction;



    void Start()
    {
        Managers.Sound.Play("AudioClip/TheNeonCity", Define.Sound.Bgm);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        _spaceBar = Managers.UI.FindDeepChild(transform, "SpaceBarCount").GetComponent<Image>();
        
        StartCoroutine(SpaceBarCheck());
        Time.timeScale = 0;
    }
   

    // 1. 스페이스바 게이지 채우기
    IEnumerator SpaceBarCheck()
    {
        while(true)
        {
            if (Input.GetKey(KeyCode.Space))
                _spaceBar.fillAmount += 0.004f;
            else if (Input.GetKeyUp(KeyCode.Space))
                _spaceBar.fillAmount = 0;

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
        Managers.UI.FindDeepChild(transform, "Recipe").gameObject.SetActive(false);
        this.transform.GetComponent<Image>().color = new Color(0,0,0,0);

        StartCoroutine(ResumeGame());
        
    }


    // 3. 카메라 움직이기
    IEnumerator ResumeGame()
    {
        CameraAction.Invoke();

        yield return WaitForRealSeconds(2f);

        Managers.UI.FindDeepChild(transform, "Ready").gameObject.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelReady_01", Define.Sound.Effect);

        yield return WaitForRealSeconds(2.5f);
        
        Time.timeScale = 1;
        Managers.UI.FindDeepChild(transform, "Ready").gameObject.SetActive(false);
        Managers.UI.FindDeepChild(transform, "Start").gameObject.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelGo", Define.Sound.Effect);  
        StartCoroutine(DisappearStartObject());
    }


    IEnumerator WaitForRealSeconds(float time)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return null;
        }
    }


    IEnumerator DisappearStartObject()
    {
        OrderStart.Invoke();
        
        yield return new WaitForSeconds(1.0f);   
        
        Managers.UI.FindDeepChild(transform, "Start").gameObject.SetActive(false);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Play();
    }

}
