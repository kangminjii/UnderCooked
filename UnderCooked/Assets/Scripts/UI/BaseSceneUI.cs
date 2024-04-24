using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BaseSceneUI : MonoBehaviour
{
    Image _image;
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
        _image = GetComponent<Image>();
        
        StartCoroutine(ChangeToDark());
    }

    IEnumerator ChangeToDark()
    {
        _image.sprite = TitleImage;

        yield return new WaitForSeconds(3f);

        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        float elapsedTime = 0f;

        // 기존 색 -> 어두운 색
        while (elapsedTime < _changeDuration)
        {
            Color lerpedColor = Color.Lerp(_startColor, _endColor, elapsedTime / _changeDuration);
            _image.color = lerpedColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        _image.sprite = UnityImage;
        elapsedTime = 0f;

        // 어두운 색 -> 기존 색
        while (elapsedTime < _changeDuration)
        {
            Color lerpedColor = Color.Lerp(_endColor, _startColor, elapsedTime / _changeDuration);
            _image.color = lerpedColor;
            
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        elapsedTime = 0f;

        // 기존 색 -> 어두운 색
        while (elapsedTime < _changeDuration)
        {
            Color lerpedColor = Color.Lerp(_startColor, _endColor, elapsedTime / _changeDuration);
            _image.color = lerpedColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        SceneManager.LoadScene(_playScene);
    }
}
