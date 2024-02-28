using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameReadyUI : MonoBehaviour
{
    Image _spaceBar;
    
    public static Action OrderStart;


    void Start()
    {
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
                break;
            }

            yield return null;
        }
    }

    // 2. 레디 문구
    void SetGameCondition()
    {
        Managers.UI.FindDeepChild(transform, "Recipe").gameObject.SetActive(false);
        this.transform.GetComponent<Image>().color = new Color(0,0,0,0);

        StartCoroutine(ResumeGame());
    }

    // 3. 시작
    IEnumerator ResumeGame()
    {
        Managers.UI.FindDeepChild(transform, "Ready").gameObject.SetActive(true);
        yield return WaitForRealSeconds(1.5f);

        Time.timeScale = 1;
        Managers.UI.FindDeepChild(transform, "Ready").gameObject.SetActive(false);
        Managers.UI.FindDeepChild(transform, "Start").gameObject.SetActive(true);
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

        yield return new WaitForSeconds(2.0f);
        Managers.UI.FindDeepChild(transform, "Start").gameObject.SetActive(false);
    }
}
