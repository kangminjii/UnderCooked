using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    // x : -342~342 , y : 9

    int _speed = 4;
    int _startPos = -342;

    List<GameObject> _orderList = new List<GameObject>();
    GridLayoutGroup _grid;



    public delegate void OrderCheck(string foodName);
    public event OrderCheck FoodOrderCheck;


    void Start()
    {
        _grid = this.GetComponent<GridLayoutGroup>();
        _grid.enabled = false;

        // 반납되는 음식이 들어올 때 이벤트 구독
        Grab_Idle.FoodOrderCheck += OrderListChecking;

        for (int i = 0; i < 3; i++)
        {
            System.Random rand = new System.Random();
            RectTransform position = MakeOrderObject(rand.Next(0, 2));

            StartCoroutine(OrderAnimation(_startPos + 70 * i, position));
        }
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

        _orderList.Add(orderObj);
        
        return orderObj.GetComponent<RectTransform>();
    }


    IEnumerator OrderAnimation(float xPos, RectTransform orderPos)
    {
        orderPos.anchoredPosition = new Vector2(342, 9); // 처음 시작 구역

        while (orderPos.anchoredPosition.x > xPos)
        {
            orderPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }

        orderPos.anchoredPosition = new Vector2(xPos, 9);
    }


    void OrderListChecking(string foodName)
    {
        if(_grid.enabled == false)
            _grid.enabled = true;


        for (int i = 0; i < _orderList.Count; i++)
        {
            if(foodName == "Prawn")
            {
                if(_orderList[i].name.Contains(foodName))
                {
                    Managers.Resource.Destroy(_orderList[i]);
                    _orderList.RemoveAt(i);
                    break;
                }
            }
            else if(foodName == "Fish")
            {
                if (_orderList[i].name.Contains(foodName))
                {
                    Managers.Resource.Destroy(_orderList[i]);
                    _orderList.RemoveAt(i);
                    break;
                }
            }
        }
    }


}
