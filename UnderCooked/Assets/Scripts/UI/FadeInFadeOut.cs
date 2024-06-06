using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInFadeOut : MonoBehaviour
{
    protected float _changeDuration = 2.0f;


    /*
     * �̹����� ���� ������ �ٲ��ִ� �Լ�
     * -> start ������ end ������ time/_changeDuration ��ŭ ��ȭ
     * -> time�� Time.deltaTime��ŭ �ð��� �߰��Ǹ� ��ȯ
     */
    protected float ChangeColor(Color start, Color end, float time, Image image)
    {
        Color lerpedColor = Color.Lerp(start, end, time / _changeDuration);
        image.color = lerpedColor;

        return time += Time.deltaTime;
    }

    public virtual IEnumerator FadeIn()
    {
        yield return null;
    }

    public virtual IEnumerator FadeOut()
    {
        yield return null;
    }



}
