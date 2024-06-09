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
     * ����Ʈ ȿ���� ���
     * FadeIn �ڷ�ƾ ����
     * DecreaseVolumeOverTime �ڷ�ƾ ����
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
     * �ð��� ���� ������ ���̴� �ڷ�ƾ
     * -> ������ ������ audioSource�� ���� duration���� targetVolume���� ���ҽ�Ŵ
     * -> ��� Sound �ʱ�ȭ
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
     * �ε����� ���� ��ȭ��Ű�� �ڷ�ƾ
     * -> �ε����� fillAmount ���� 1�� �ɶ����� Time.deltaTime�� ���� ��ȭ��Ŵ
     * -> FadeOut �ڷ�ƾ ����
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
     * FadeIn�ϴ� �ڷ�ƾ override
     * -> �θ��� FadeIn �ڷ�ƾ ����
     */
    public override IEnumerator FadeIn(Color start, Color end, Image image)
    {
        yield return base.FadeIn(start, end, image);
    }


    /*
    * FadeOut�ϴ� �ڷ�ƾ override
    * -> 1�� �� ����Ʈ ���� ���
    * -> �θ��� FadeOut �ڷ�ƾ ����
    * -> PlayerPrefs�� ����� ���� �� �̸��� ������ �ε�
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
