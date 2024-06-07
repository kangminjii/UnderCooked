using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeInFadeOut : MonoBehaviour
{
    protected float _changeDuration = 2.0f;


    /*
     * 이미지의 색을 서서히 바꿔주는 함수
     * -> 이미지의 색이 start 색에서 end 색까지 time/_changeDuration 만큼 변화함
     * -> time에 Time.deltaTime만큼 시간이 추가되며 반환
     */
    protected float ChangeColor(Color start, Color end, float time, Image image)
    {
        Color lerpedColor = Color.Lerp(start, end, time / _changeDuration);
        image.color = lerpedColor;

        return time += Time.deltaTime;
    }


    /*
     * FadeIn 코루틴
     * -> _changeDuration만큼 ChangeColor함수를 통해 색이 바뀜
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
    * FadeOut 코루틴
    * -> _changeDuration만큼 ChangeColor함수를 통해 색이 바뀜
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
