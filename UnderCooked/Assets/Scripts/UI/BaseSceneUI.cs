using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseSceneUI : FadeInFadeOut
{
    Color   _startColor = Color.white;
    Color   _endColor = Color.black;
    string  _playScene = "[1]Start";
    int     _width = 1920;
    int     _height = 1080;
    
    [SerializeField]
    Image   _image;
    [SerializeField]
    Sprite  _changeImage;


    /*
     * 해상도 크기 고정
     * FadeOut 코루틴 실행
     */
    void Awake()
    {
        Screen.SetResolution(_width, _height, true);
        StartCoroutine(FadeOut(_startColor, _endColor, _image));
    }


    /*
     * FadeOut하는 코루틴 override
     * -> 3초후 배경음 재생
     * -> 부모의 FadeOut 코루틴 실행
     * -> 2초후 FadeIn 시작
     */
    public override IEnumerator FadeOut(Color start, Color end, Image image)
    {
        yield return new WaitForSeconds(3f);

        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        yield return base.FadeOut(start, end, image);
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(FadeIn(start, end, image));
    }


    /*
     * FadeIn하는 코루틴 override
     * -> 이미지 교체
     * -> 부모의 FadeIn 코루틴 실행
     * -> 부모의 FadeOut 코루틴 실행
     * -> 다음 씬 로드
     */
    public override IEnumerator FadeIn(Color start, Color end, Image image)
    {
        _image.sprite = _changeImage;

        yield return base.FadeIn(start, end, image);
        yield return base.FadeOut(start, end, image);
        
        SceneManager.LoadScene(_playScene);
    }
}