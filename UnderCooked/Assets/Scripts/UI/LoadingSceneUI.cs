using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneUI : FadeInFadeOut
{
    Color _startColor = new Color(0,0,0,0);
    Color _endColor = Color.black;
    [SerializeField]
    Image _loadingBar;
    [SerializeField]
    Image _image;


    /*
     * 이펙트 효과음 재생
     * FadeIn 코루틴 실행
     * DecreaseVolumeOverTime 코루틴 실행
     */
    void Awake()
    {
        AudioSource bgmAudioSource = Managers.Sound.AudioSources[(int)Define.Sound.Bgm];
        
        Managers.Sound.Play("AudioClip/UI_Screen_In", Define.Sound.Effect);

        StartCoroutine(FadeIn(_startColor, _endColor, _image));
        StartCoroutine(DecreaseVolumeOverTime(bgmAudioSource, 0f, 2.5f));
        StartCoroutine(LoadingBar());
    }


    /*
     * 시간에 따라 볼륨을 줄이는 코루틴
     * -> 볼륨을 조절할 audioSource의 값을 duration동안 targetVolume까지 감소시킴
     * -> 모든 Sound 초기화
     */
    IEnumerator DecreaseVolumeOverTime(AudioSource audioSource, float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Managers.Sound.Clear();
    }


    /*
     * 로딩바의 값을 변화시키는 코루틴
     * -> 로딩바의 fillAmount 값이 1이 될때까지 Time.deltaTime을 더해 변화시킴
     * -> FadeOut 코루틴 실행
     */
    IEnumerator LoadingBar()
    {
        while(_loadingBar.fillAmount < 1)
        {
            _loadingBar.fillAmount += 0.5f * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeOut(_startColor, _endColor, _image));
    }


    /*
     * FadeIn하는 코루틴 override
     * -> 부모의 FadeIn 코루틴 실행
     */
    public override IEnumerator FadeIn(Color start, Color end, Image image)
    {
        yield return base.FadeIn(start, end, image);
    }


    /*
    * FadeOut하는 코루틴 override
    * -> 1초 후 이펙트 사운드 재생
    * -> 부모의 FadeOut 코루틴 실행
    * -> PlayerPrefs에 저장된 다음 씬 이름을 가져와 로드
    */
    public override IEnumerator FadeOut(Color start, Color end, Image image)
    {
        yield return new WaitForSeconds(1.0f);

        Managers.Sound.Play("AudioClip/UI_Screen_Out", Define.Sound.Effect);
        Managers.Sound.Play("AudioClip/Tutorial_Pop_In", Define.Sound.Effect, 1f , 0.2f);

        yield return base.FadeOut(start, end, image);

        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
    }

}
