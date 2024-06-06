using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingUI : FadeInFadeOut
{
    Color _startColor = new Color(0,0,0,0);
    Color _endColor = Color.black;
    [SerializeField]
    Image _loadingBar;
    [SerializeField]
    Image _image;


    void Awake()
    {
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


    IEnumerator LoadingBar()
    {
        while(_loadingBar.fillAmount < 1)
        {
            _loadingBar.fillAmount += 0.5f * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeOut());
    }


    public override IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_endColor, _startColor, elapsedTime, _image);
            yield return null;
        }
    }


    public override IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.0f);

        Managers.Sound.Play("AudioClip/UI_Screen_Out", Define.Sound.Effect);
        Managers.Sound.Play("AudioClip/Tutorial_Pop_In", Define.Sound.Effect, 1f , 0.2f);

        float elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_startColor, _endColor, elapsedTime, _image);
            yield return null;
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
    }

}
