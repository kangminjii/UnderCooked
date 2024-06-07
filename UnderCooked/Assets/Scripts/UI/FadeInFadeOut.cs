using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeInFadeOut : MonoBehaviour
{
    protected float _changeDuration = 2.0f;


    /*
     * �̹����� ���� ������ �ٲ��ִ� �Լ�
     * -> �̹����� ���� start ������ end ������ time/_changeDuration ��ŭ ��ȭ��
     * -> time�� Time.deltaTime��ŭ �ð��� �߰��Ǹ� ��ȯ
     */
    protected float ChangeColor(Color start, Color end, float time, Image image)
    {
        Color lerpedColor = Color.Lerp(start, end, time / _changeDuration);
        image.color = lerpedColor;

        return time += Time.deltaTime;
    }


    /*
     * FadeIn �ڷ�ƾ
     * -> _changeDuration��ŭ ChangeColor�Լ��� ���� ���� �ٲ�
     */
    public virtual IEnumerator FadeIn(Color start, Color end, Image image)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(end, start, elapsedTime, image);
            yield return null;
        }
    }


    /*
    * FadeOut �ڷ�ƾ
    * -> _changeDuration��ŭ ChangeColor�Լ��� ���� ���� �ٲ�
    */
    public virtual IEnumerator FadeOut(Color start, Color end, Image image)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < _changeDuration)
        {
            elapsedTime = ChangeColor(start, end, elapsedTime, image);
            yield return null;
        }
    }
}
