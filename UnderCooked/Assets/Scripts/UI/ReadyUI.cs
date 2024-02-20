using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUI : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ResumeGame());
        Time.timeScale = 0;
    }
   
    IEnumerator ResumeGame()
    {
        yield return WaitForRealSeconds(3.0f);
        Time.timeScale = 1;
        StartCoroutine(DisappearStartObject());
    }

    IEnumerator WaitForRealSeconds(float time)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return null;
        }

        Managers.UI.FindDeepChild(transform, "Ready").gameObject.SetActive(false);
        Managers.UI.FindDeepChild(transform, "Start").gameObject.SetActive(true);
    }

    IEnumerator DisappearStartObject()
    {
        yield return new WaitForSeconds(2.0f);
        Managers.UI.FindDeepChild(transform, "Start").gameObject.SetActive(false);
    }
}
