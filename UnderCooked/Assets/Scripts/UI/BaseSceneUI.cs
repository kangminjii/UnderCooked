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
     * �ػ� ũ�� ����
     * FadeOut �ڷ�ƾ ����
     */
    void Awake()
    {
        Screen.SetResolution(_width, _height, true);
        StartCoroutine(FadeOut(_startColor, _endColor, _image));
    }


    /*
     * FadeOut�ϴ� �ڷ�ƾ override
     * -> 3���� ����� ���
     * -> �θ��� FadeOut �ڷ�ƾ ����
     * -> 2���� FadeIn ����
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
     * FadeIn�ϴ� �ڷ�ƾ override
     * -> �̹��� ��ü
     * -> �θ��� FadeIn �ڷ�ƾ ����
     * -> �θ��� FadeOut �ڷ�ƾ ����
     * -> ���� �� �ε�
     */
    public override IEnumerator FadeIn(Color start, Color end, Image image)
    {
        _image.sprite = _changeImage;

        yield return base.FadeIn(start, end, image);
        yield return base.FadeOut(start, end, image);
        
        SceneManager.LoadScene(_playScene);
    }
}