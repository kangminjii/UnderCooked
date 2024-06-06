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
     * �ػ� ũ�� ���� �� �ڷ�ƾ���� FadeInFadeOut ����
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
     * FadeOut�ϴ� �ڷ�ƾ
     * -> 3���� ����� ���
     * -> �̹����� ������ ���->�������� ��ȭ
     * -> 2���� ���� �ڷ�ƾ ����
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
     * �̹����� �����ϰ� FadeInFadeOut�ϴ� �ڷ�ƾ 
     * -> �̹����� ������ ����->���->�������� ��ȭ
     * -> ���� ���� �ε��Ѵ�
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