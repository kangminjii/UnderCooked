using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    // animation
    int _speed = 4;
    int _startPos = -342;
    GridLayoutGroup _grid;
    GameObject _orderPanel;

    // score
    int _addingScore = 100;
    Text _scoreText;
    GameObject _scorePanel;
    public int TotalScore = 0;



    public List<GameObject> OrderList = new List<GameObject>();

    public delegate void OrderCheck(string foodName);
    public static event OrderCheck FoodOrderCheck;
    public static Action<bool> TimeStart;


    void Start()
    {
        _orderPanel = Managers.UI.FindDeepChild(transform, "Order_Panel").gameObject;
        _scorePanel = Managers.UI.FindDeepChild(transform, "Score_Panel").gameObject;
        _scoreText = Managers.UI.FindDeepChild(_scorePanel.transform, "Score").GetComponent<Text>();
        _grid = _orderPanel.GetComponent<GridLayoutGroup>();
        
        
        for (int i = 0; i < 2; i++)
        {
            System.Random rand = new System.Random();
            RectTransform position = MakeOrderObject(rand.Next(0, 2));

            StartCoroutine(OrderAnimation(_startPos + 70 * i, position));
        }

        _grid.enabled = false;

        Grab_Idle.FoodOrderCheck += OrderListChecking;
        Grab_Moving.FoodOrderCheck += OrderListChecking;

    }


    void OnDestroy()
    {
        Grab_Idle.FoodOrderCheck -= OrderListChecking;
        Grab_Moving.FoodOrderCheck -= OrderListChecking;
    }


    RectTransform MakeOrderObject(int num)
    {
        GameObject orderObj;

        if (num == 0)
            orderObj = Managers.Resource.Instantiate("Fish_Order", null, null, _orderPanel.transform);
        else
            orderObj = Managers.Resource.Instantiate("Prawn_Order", null, null, _orderPanel.transform);

        OrderList.Add(orderObj);
        
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


        for (int i = 0; i < OrderList.Count; i++)
        {
            if(foodName == "Prawn" || foodName == "Fish")
            {
                if(OrderList[i].name.Contains(foodName))
                {
                    Managers.Resource.Destroy(OrderList[i]);
                    OrderList.RemoveAt(i);

                    TotalScore += _addingScore;
                    _scoreText.text = TotalScore.ToString();

                    TimeStart.Invoke(true);
                    break;
                }
            }
        }
    }

}
