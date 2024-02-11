using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    // x : -342~342 , y : 9

    float _speed = 4.0f;

    List<GameObject> OrderList = new List<GameObject>();


    public delegate void OrderCheck(GameObject foodCheck);
    public event OrderCheck FoodOrderCheck;



    void Start()
    {
        // �ݳ��Ǵ� ������ ���� �� �̺�Ʈ ����
        Grab_Idle.FoodOrderCheck += OrderListChecking;

        StartCoroutine(OrderAnimation(-342, MakeOrderObject(0)));
        StartCoroutine(OrderAnimation(-272, MakeOrderObject(1)));
        StartCoroutine(OrderAnimation(-202, MakeOrderObject(1)));


    }

    void OnDestroy()
    {
        Grab_Idle.FoodOrderCheck -= OrderListChecking;
    }


    RectTransform MakeOrderObject(int num)
    {
        GameObject orderObj;

        if (num == 0)
            orderObj = Managers.Resource.Instantiate("Fish_Order", null, null, this.transform);
        else
            orderObj = Managers.Resource.Instantiate("Prawn_Order", null, null, this.transform);

        OrderList.Add(orderObj);
        
        return orderObj.GetComponent<RectTransform>();
    }


    IEnumerator OrderAnimation(float xPos, RectTransform orderPos)
    {
        orderPos.anchoredPosition = new Vector2(342, 9); // ó�� ���� ����

        while (orderPos.anchoredPosition.x > xPos)
        {
            orderPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }

        orderPos.anchoredPosition = new Vector2(xPos, 9);
    }


    void OrderListChecking(GameObject foodCheck)
    {
        for(int i = 0; i < OrderList.Count; i++)
        {
            Debug.Log("�ֹ� üũ : " + foodCheck.name);
        }
    }
}
