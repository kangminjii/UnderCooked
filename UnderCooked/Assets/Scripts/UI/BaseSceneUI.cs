using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BaseSceneUI : MonoBehaviour
{
    public Image _image;
    Color _startColor = Color.white;
    Color _endColor = Color.black;
    float _changeDuration = 2.0f;
    string _playScene = "[1]Start";
    int _width = 1920;
    int _height = 1080;

    public Sprite TitleImage;
    public Sprite UnityImage;


    void Start()
    {
        Screen.SetResolution(_width, _height, true);
        //_image = GetComponent<Image>();
        
        StartCoroutine(ChangeToDark());
    }

    IEnumerator ChangeToDark()
    {
        _image.sprite = TitleImage;

        yield return new WaitForSeconds(3f);

        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        float elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_startColor, _endColor, elapsedTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        _image.sprite = UnityImage;
        elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_endColor, _startColor, elapsedTime);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(_startColor, _endColor, elapsedTime);
            yield return null;
        }

        SceneManager.LoadScene(_playScene);
    }

    float ChangeColor(Color start, Color end, float time)
    {
        Color lerpedColor = Color.Lerp(start, end, time / _changeDuration);
        _image.color = lerpedColor;

        return time += Time.deltaTime;
    }
}