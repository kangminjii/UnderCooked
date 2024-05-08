using System.Collections;
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
        _loadingBar = Define.FindDeepChild(transform, "Filled").GetComponent<Image>();
        _fadeInOut = Define.FindDeepChild(transform, "FadeInOut").GetComponent<Image>();

        Managers.Sound.Play("AudioClip/UI_Screen_In", Define.Sound.Effect);

        StartCoroutine(FadeIn());
        StartCoroutine(LoadingBar());

        AudioSource bgmAudioSource = Managers.Sound.AudioSources[(int)Define.Sound.Bgm];
        StartCoroutine(DecreaseVolumeOverTime(bgmAudioSource, 0f, 2.5f));
    }


    IEnumerator DecreaseVolumeOverTime(AudioSource audioSource, float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        Managers.Sound.Clear();
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

        Managers.Sound.Play("AudioClip/UI_Screen_Out", Define.Sound.Effect);
        Managers.Sound.Play("AudioClip/Tutorial_Pop_In", Define.Sound.Effect, 1f , 0.2f);

        while (_fadeInOut.color.a < 1)
        {
            temp.a += _speed;
            _fadeInOut.color = temp;

            yield return null;
        }
        
        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
    }

}
