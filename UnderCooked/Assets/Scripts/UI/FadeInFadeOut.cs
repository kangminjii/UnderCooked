using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInFadeOut : MonoBehaviour
{
    protected float _changeDuration = 2.0f;


    /*
     * 이미지의 색을 서서히 바꿔주는 함수
     * -> start 색에서 end 색까지 time/_changeDuration 만큼 변화
     * -> time에 Time.deltaTime만큼 시간이 추가되며 반환
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
