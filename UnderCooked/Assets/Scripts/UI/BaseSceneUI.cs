using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseSceneUI : FadeInFadeOut
{
    Color   _startColor = Color.white;
    Color   _endColor = Color.black;
    //float   _changeDuration = 2.0f;
    string  _playScene = "[1]Start";
    int     _width = 1920;
    int     _height = 1080;
    [SerializeField]
    Image   _image;
    [SerializeField]
    Sprite  _changeImage;


    /*
     * 해상도 크기 고정 및 코루틴으로 FadeInFadeOut 실행
     */
    void Awake()
    {
        Screen.SetResolution(_width, _height, true);
        StartCoroutine(FadeOut());
    }


    public override IEnumerator FadeIn()
    {
        yield return null;
    }


    /*
     * FadeOut하는 코루틴
     * -> 3초후 배경음 재생
     * -> 이미지의 색상을 흰색->검정으로 변화
     * -> 2초후 다음 코루틴 시작
     */
    public override IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3f);

        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        float elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_startColor, _endColor, elapsedTime, _image);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeInFadeOut());
    }


    /*
     * 이미지를 변경하고 FadeInFadeOut하는 코루틴 
     * -> 이미지의 색상을 검정->흰색->검정으로 변화
     * -> 다음 씬을 로드한다
     */
    IEnumerator FadeInFadeOut()
    {
        _image.sprite = _changeImage;

        float elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_endColor, _startColor, elapsedTime, _image);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_startColor, _endColor, elapsedTime, _image);
            yield return null;
        }

        SceneManager.LoadScene(_playScene);
    }

}