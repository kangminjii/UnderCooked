using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class OrderUI : MonoBehaviour
{
    // animation
    int _speed = 4;
    int _startPos = -342;
    GridLayoutGroup _grid;
    GameObject _orderPanel;

    // score
    int _addingScore = 20;
    int _minusScore = 10;
    int _totalScore = 0;
    int _successFood = 0;
    int _failFood = 0;
    Text _scoreText;
    GameObject _scorePanel;

    // orderUpdate
    float _updateTime = 15;

    public List<GameObject> OrderList = new List<GameObject>();

    public delegate void OrderCheck(string foodName);
    public event OrderCheck FoodOrderCheck;
    bool orderCheck = false;


    void Start()
    {
        _orderPanel = Managers.UI.FindDeepChild(transform, "Order_Panel").gameObject;
        _scorePanel = Managers.UI.FindDeepChild(transform, "Score_Panel").gameObject;
        _scoreText = Managers.UI.FindDeepChild(_scorePanel.transform, "Score").GetComponent<Text>();
        _grid = _orderPanel.GetComponent<GridLayoutGroup>();
        _grid.enabled = false;

        AddOrderList(2);

        Grab_Idle.FoodOrderCheck += OrderListChecking;
        Grab_Moving.FoodOrderCheck += OrderListChecking;

        StartCoroutine(OrderListUpdate());

        PlayerPrefs.DeleteAll();
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


    IEnumerator OrderAnimation1(float xPos, RectTransform orderPos)
    {
        orderPos.anchoredPosition = new Vector2(342, 9); // 처음 시작 구역

        while (orderPos.anchoredPosition.x > xPos)
        {
            orderPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }
    }

    IEnumerator OrderAnimation2(float xPos, RectTransform orderPos)
    {
        yield return new WaitForSeconds(0.1f);

        _grid.enabled = false;
        orderPos.anchoredPosition = new Vector2(342, 9); // 처음 시작 구역

        while (orderPos.anchoredPosition.x > xPos)
        {
            orderPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }

        _grid.enabled = true;
    }

    void AddOrderList(int num)
    {
        // 처음 시작할때는 OrderAnimation1
        if (num == 2)
        {
            for (int i = 0; i < num; i++)
            {
                System.Random rand = new System.Random();
                RectTransform position = MakeOrderObject(rand.Next(0, 2));
                StartCoroutine(OrderAnimation1(_startPos + 70 * (OrderList.Count - 1), position));
            }
        }
        // 이후에는 OrderAnimation2
        else
        {
            System.Random rand = new System.Random();
            RectTransform position = MakeOrderObject(rand.Next(0, 2));
            StartCoroutine(OrderAnimation2(_startPos + 70 * (OrderList.Count - 1), position));
        }
    }


    // 주문서 갱신 조건
    // 1. 최소 2개 이상 유지
    // 2. UpdateTime마다 1개씩 추가
    private void Update()
    {
        if (OrderList.Count < 2)
        {
            AddOrderList(1);
        }

        PlayerPrefs.Save();
    }

    IEnumerator OrderListUpdate()
    {
        yield return new WaitForSeconds(_updateTime);
        
        AddOrderList(1);
        StartCoroutine(OrderListUpdate());
    }


    // 플레이어가 반납한 음식 판단하는 함수
    void OrderListChecking(string foodName)
    {
        if(_grid.enabled == false)
            _grid.enabled = true;


        for (int i = 0; i < OrderList.Count; i++)
        {
            if(foodName == "Prawn" || foodName == "Fish")
            {
                if (OrderList[i].name.Contains(foodName))
                {
                    Managers.Resource.Destroy(OrderList[i]);
                    OrderList.RemoveAt(i);

                    _totalScore += _addingScore;
                    _scoreText.text = _totalScore.ToString();

                    GameObject Passing = GameObject.Find("m_sk_the_pass_red_01_2");
                    Managers.Resource.Instantiate("OrderEffect", Passing.transform.position + new Vector3(-1f, 1f, 0f), Quaternion.identity);
                    Managers.Sound.Play("AudioClip/Order_Successful", Define.Sound.Effect);

                    _successFood++;
                    PlayerPrefs.SetInt("Success", _successFood);

                    orderCheck = true;

                    break;
                }
            }
        }

        if (!orderCheck)
        {
            _totalScore -= _minusScore;
            if (_totalScore < 0)
                _totalScore = 0;
            _scoreText.text = _totalScore.ToString();

            _failFood++;
            PlayerPrefs.SetInt("Fail", _failFood);

            Managers.Sound.Play("AudioClip/Order_Fail", Define.Sound.Effect);
        }
    }

}
