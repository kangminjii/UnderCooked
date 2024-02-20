using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingUI : MonoBehaviour
{
    Image _loadingBar;
    Image _fadeInOut;
    float _speed = 0.003f;

    void Start()
    {
        _loadingBar = Managers.UI.FindDeepChild(transform, "Filled").GetComponent<Image>();
        _fadeInOut = Managers.UI.FindDeepChild(transform, "FadeInOut").GetComponent<Image>();

        StartCoroutine(FadeIn());
        StartCoroutine(LoadingBar());
    }


    IEnumerator FadeIn()
    {
        Color temp = _fadeInOut.color;

        while (_fadeInOut.color.a > 0)
        {
            temp.a -= _speed;
            _fadeInOut.color = temp;
            yield return null;
        }

    }
    

    IEnumerator LoadingBar()
    {
        while(_loadingBar.fillAmount < 1)
        {
            _loadingBar.fillAmount += 0.5f * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.0f);

        Color temp = _fadeInOut.color;

        while (_fadeInOut.color.a < 1)
        {
            temp.a += _speed;
            _fadeInOut.color = temp;
            yield return null;
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
    }

}
