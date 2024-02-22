using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ImageFadeinout : MonoBehaviour
{
    public Image image;
    public Sprite UnitySprite;

    string _playScene = "[1]Start";

    Color startColor = Color.white; // 밝은 색
    Color endColor = Color.black;   // 어두운 색
    float changeDuration = 2.0f;    // 변화 지속 시간
    float delayBeforeRestore = 2f; // 복원되기 전 딜레이 시간

    void Start()
    {
        StartCoroutine(ChangeToDark());

    }

    IEnumerator ChangeToDark()
    {

        yield return new WaitForSeconds(3f);

        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        float elapsedTime = 0f;

        // 밝은 색에서 어두운 색으로 변화
        while (elapsedTime < changeDuration)
        {
            Color lerpedColor = Color.Lerp(startColor, endColor, elapsedTime / changeDuration);
            image.color = lerpedColor;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // 어두운 색 유지 후 원래 색상으로 복원
        yield return new WaitForSeconds(delayBeforeRestore);
        image.sprite = UnitySprite;
        elapsedTime = 0f;
        while (elapsedTime < changeDuration)
        {
            Color lerpedColor = Color.Lerp(endColor, startColor, elapsedTime / changeDuration);
            image.color = lerpedColor;
            yield return null;
            elapsedTime += Time.deltaTime;
        }


        elapsedTime = 0f;
        while (elapsedTime < changeDuration)
        {
            Color lerpedColor = Color.Lerp(startColor, endColor, elapsedTime / changeDuration);
            image.color = lerpedColor;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        SceneManager.LoadScene(_playScene);
    }

}
