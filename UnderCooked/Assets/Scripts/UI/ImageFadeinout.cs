using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ImageFadeinout : MonoBehaviour
{
    public Image image;
    public Sprite UnitySprite;

    string _playScene = "[1]Start";

    Color startColor = Color.white; // ���� ��
    Color endColor = Color.black;   // ��ο� ��
    float changeDuration = 2.0f;    // ��ȭ ���� �ð�
    float delayBeforeRestore = 2f; // �����Ǳ� �� ������ �ð�

    void Start()
    {
        StartCoroutine(ChangeToDark());

    }

    IEnumerator ChangeToDark()
    {

        yield return new WaitForSeconds(3f);

        Managers.Sound.Play("AudioClip/Frontend", Define.Sound.Bgm);

        float elapsedTime = 0f;

        // ���� ������ ��ο� ������ ��ȭ
        while (elapsedTime < changeDuration)
        {
            Color lerpedColor = Color.Lerp(startColor, endColor, elapsedTime / changeDuration);
            image.color = lerpedColor;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // ��ο� �� ���� �� ���� �������� ����
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
