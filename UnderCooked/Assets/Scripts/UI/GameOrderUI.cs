using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOrderUI : MonoBehaviour
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
    float _updateTime = 10f;
    float _orderWaitingTime = 40f;
    int _orderNumber = 0;
    bool _animationCheck;
    bool _orderCheck = false;

    List<KeyValuePair<GameObject, int>> OrderList = new List<KeyValuePair<GameObject, int>>();


    public delegate void OrderCheck(string foodName);
    public event OrderCheck FoodOrderCheck;

    public delegate void OrderUIStart();
    public event OrderUIStart OrderStart;


    private void Start()
    {
        GameReadyUI.OrderAction += SceneStart;
    }


    void SceneStart()
    {
        PlayerPrefs.DeleteAll();

        _orderPanel = Define.FindDeepChild(transform, "Order_Panel").gameObject;
        _scorePanel = Define.FindDeepChild(transform, "Score_Panel").gameObject;
        _scoreText = Define.FindDeepChild(_scorePanel.transform, "Score").GetComponent<Text>();
        _grid = _orderPanel.GetComponent<GridLayoutGroup>();
        _grid.enabled = false;

        Player.FoodOrderCheck += OrderListChecking;

        AddOrderList(2);
        StartCoroutine(OrderListUpdate(_updateTime));
    }


    void OnDestroy()
    {
        Player.FoodOrderCheck -= OrderListChecking;
        GameReadyUI.OrderAction -= SceneStart;
    }


    RectTransform MakeOrderObject(int num)
    {
        GameObject orderObj;

        if (num == 0)
            orderObj = Managers.Resource.Instantiate("Fish_Order", null, null, _orderPanel.transform);
        else
            orderObj = Managers.Resource.Instantiate("Prawn_Order", null, null, _orderPanel.transform);

        _orderNumber++;
        OrderList.Add(new KeyValuePair<GameObject, int>(orderObj, _orderNumber));
        StartCoroutine(OrderObjectProgress(orderObj, _orderNumber));

        return orderObj.GetComponent<RectTransform>();
    }


    IEnumerator OrderObjectProgress(GameObject obj, int _orderNumber)
    {
        Transform progressBar = Define.FindDeepChild(obj.transform, "ProgressBar");
        float waitingAmount = progressBar.GetComponent<Image>().fillAmount;

        // 1초간 게이지 줄어듦
        while (waitingAmount > 0 && obj != null)
        {
            waitingAmount -= 1 / _orderWaitingTime;
            progressBar.GetComponent<Image>().fillAmount = waitingAmount;

            // 색 변화
            if (waitingAmount < 0.7f && waitingAmount > 0.5f)
            {
                Color changeColor = new Color(0.4f, 115 / 256f, 14 / 256f);
                progressBar.GetComponent<Image>().color = changeColor;
            }
            else if (waitingAmount <= 0.5f && waitingAmount > 0.3f)
            {
                Color changeColor = new Color(0.6f, 115 / 256f, 14 / 256f);
                progressBar.GetComponent<Image>().color = changeColor;
            }
            else if (waitingAmount <= 0.3f && waitingAmount > 0.1f)
            {
                Color changeColor = new Color(0.8f, 115 / 256f, 14 / 256f);
                progressBar.GetComponent<Image>().color = changeColor;
            }
            else if (waitingAmount <= 0.1f)
            {
                Color changeColor = Color.red;
                progressBar.GetComponent<Image>().color = changeColor;
            }

            yield return new WaitForSeconds(1);
        }


        // 시간 초과
        if (waitingAmount <= 0)
        {
            OrderFail();

            StartCoroutine(FailOrderDestroy(obj));
            OrderList.Remove(new KeyValuePair<GameObject, int>(obj, _orderNumber));
        }
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
        _grid.enabled = false;
        _animationCheck = false;

        orderPos.anchoredPosition = new Vector2(342, 9); // 처음 시작 구역

        while (orderPos.anchoredPosition.x > xPos)
        {
            orderPos.anchoredPosition -= new Vector2(_speed, 0);
            yield return null;
        }

        _grid.enabled = true;
        _animationCheck = true;
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
        if (Time.timeScale > 0)
        {
            if (OrderList.Count < 2 && _animationCheck)
            {
                AddOrderList(1);
            }

            PlayerPrefs.Save();
        }
    }


    IEnumerator OrderListUpdate(float updateTime)
    {
        yield return new WaitForSeconds(updateTime);

        AddOrderList(1);
        StartCoroutine(OrderListUpdate(updateTime));
    }


    // 플레이어가 반납한 음식 판단하는 함수
    void OrderListChecking(string foodName)
    {
        for (int i = 0; i < OrderList.Count; i++)
        {
            if (foodName == "Prawn" || foodName == "Fish")
            {
                if (OrderList[i].Key.name.Contains(foodName))
                {
                    StartCoroutine(SuccessOrderDestroy(OrderList[i].Key));
                    OrderList.Remove(OrderList[i]);

                    _totalScore += _addingScore;
                    _scoreText.text = _totalScore.ToString();

                    GameObject Passing = GameObject.Find("m_sk_the_pass_red_01_2");
                    Managers.Resource.Instantiate("OrderEffect", Passing.transform.position + new Vector3(-1f, 1f, 0f), Quaternion.identity);
                    Managers.Sound.Play("AudioClip/Order_Successful", Define.Sound.Effect);

                    _successFood++;
                    PlayerPrefs.SetInt("Success", _successFood);

                    _orderCheck = true;

                    if (_grid.enabled == false)
                        _grid.enabled = true;

                    break;
                }
                else
                    _orderCheck = false;
            }
        }


        if (!_orderCheck)
        {
            OrderFail();
        }
    }
    
    void OrderFail()
    {
        _totalScore -= _minusScore;
        if (_totalScore < 0)
            _totalScore = 0;
        _scoreText.text = _totalScore.ToString();

        _failFood++;
        PlayerPrefs.SetInt("Fail", _failFood);

        Managers.Sound.Play("AudioClip/Order_Fail", Define.Sound.Effect);

        for (int i = 0; i < OrderList.Count; i++)
        {
            StartCoroutine(EntireFailed(i));
        }

    } 

    IEnumerator SuccessOrderDestroy(GameObject go)
    {
        Transform effectImage = Define.FindDeepChild(go.transform, "Success");
        effectImage.gameObject.SetActive(true);
        
        float alpha = 0;

        while (alpha < 0.8f)
        {
            alpha += 0.2f; 
            Color changeColor = new Color(0, 1, 0, alpha);
            effectImage.GetComponent<Image>().color = changeColor;
          
            yield return new WaitForSeconds(0.05f);
        }

        Managers.Resource.Destroy(go);
    }

    IEnumerator FailOrderDestroy(GameObject go)
    {
        Transform effectImage = Define.FindDeepChild(go.transform, "Fail");
        effectImage.gameObject.SetActive(true);

        float alpha = 0;

        while (alpha < 0.8f)
        {
            alpha += 0.2f;
            Color changeColor = new Color(1, 0, 0, alpha);
            effectImage.GetComponent<Image>().color = changeColor;

            yield return new WaitForSeconds(0.05f);
        }

        Managers.Resource.Destroy(go);
    }

    IEnumerator EntireFailed(int num)
    {
        Transform effectImage = Define.FindDeepChild(OrderList[num].Key.transform, "Fail");
        effectImage.gameObject.SetActive(true);

        float alpha = 0;

        while (alpha < 0.8f)
        {
            alpha += 0.2f;
            Color changeColor = new Color(1, 0, 0, alpha);
            effectImage.GetComponent<Image>().color = changeColor;

            yield return new WaitForSeconds(0.05f);
        }

        effectImage.gameObject.SetActive(false);
    }

}
