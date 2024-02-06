using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    // x : -342~342 , y : 9

    RectTransform _prawnPos;

    float _speed = 3.0f;
    float _maxLength = 342.0f;
    float _distance = 70.0f;
    float _originHeight = 9.0f;

    List<GameObject> OrderList = new List<GameObject>();


    public GameObject Fish_Order;
    public GameObject Prawn_Order;
    public delegate void OrderCheck();
    public static event OrderCheck FoodOrderCheck;


    GameObject test_fish;
    RectTransform _fishPos;


    void Start()
    {
        // 초반 위치 설정
        //_fishPos = Fish_Order.GetComponent<RectTransform>();
        //_prawnPos = Prawn_Order.GetComponent<RectTransform>();

        //_fishPos.anchoredPosition = new Vector2(_maxLength - _distance * 1, 9);     // #1
        //_prawnPos.anchoredPosition = new Vector2(_maxLength - _distance * 0, 9);    // #2

        // 리스트 추가
        //OrderList.Add(Fish_Order);
        //OrderList.Add(Prawn_Order);

        // 반납되는 음식이 들어올 때 이벤트 구독
        PassingGate.FoodOrderCheck += OrderListChecking;

        // distance는 list의 index값에 따라 달라짐
        //StartCoroutine(PrawnOrderAnimation(-_maxLength + _distance));



        test_fish = Managers.Resource.Instantiate("Fish_Order", new Vector2(1920,970), null, this.transform);
        OrderList.Add(test_fish);
        _fishPos = test_fish.GetComponent<RectTransform>();
        StartCoroutine(FishOrderAnimation(-342));

    }

    void OnDestroy()
    {
        PassingGate.FoodOrderCheck -= OrderListChecking;
    }


    IEnumerator FishOrderAnimation(float distance)
    {
        //while(_fishPos.anchoredPosition.x > distance)
        //{
        //    _fishPos.anchoredPosition -= new Vector2(_speed, 0);
        //    yield return null;
        //}

        //_fishPos.anchoredPosition = new Vector2(distance, _originHeight);

        while (_fishPos.anchoredPosition.x > distance)
        {
            _fishPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }

        _fishPos.anchoredPosition = new Vector2(distance, 9);
    }

    IEnumerator PrawnOrderAnimation(float distance)
    {
        while (_prawnPos.anchoredPosition.x > distance)
        {
            _prawnPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }

        _prawnPos.anchoredPosition = new Vector2(distance, _originHeight);
    }

    void OrderListChecking()
    {
        for(int i = 0; i < OrderList.Count; i++)
        {
            Debug.Log("주문 체크");
        }
    }
}
