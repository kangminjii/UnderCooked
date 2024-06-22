using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOrderUI : MonoBehaviour
{
    int         _startPos = -342;
    int         _addScore = 20;
    int         _minusScore = 10;
    int         _totalScore = 0;
    int         _successFood = 0;
    int         _failFood = 0;
    int         _orderNumber = 0;
    float       _animationSpeed = 4f;
    float       _updateTime = 7.5f;
    float       _orderWaitingTime = 40f;
    bool        _orderCheck = false;
    List<KeyValuePair<GameObject, int>> _orderList = new List<KeyValuePair<GameObject, int>>();
    
    [SerializeField]
    GameObject  _orderPrefabs;
    [SerializeField]
    Text        _scoreText;


    /*
     * 구독 및 해제
     * -> GameSceneUI에서 게임이 시작될 때 OrderStart 함수 실행
     * -> 음식을 제출할 때 OrderListChecking 함수 실행
     */
    private void Awake()
    {
        GameSceneUI.OrderAction += OrderStart;
        Player.FoodOrderCheck += OrderListChecking;
    }


    void OnDestroy()
    {
        Player.FoodOrderCheck -= OrderListChecking;
        GameSceneUI.OrderAction -= OrderStart;
    }


    /*
     * 게임이 시작될 때 호출되는 함수
     * -> PlayerPrefs 초기화
     * -> 주문서 2개 생성
     * -> _updateTime마다 호출되는 재귀 코루틴 실행
     */
    void OrderStart()
    {
        PlayerPrefs.DeleteAll();
        AddOrderList(2);
        StartCoroutine(OrderListUpdate(_updateTime));
    }


    /*
     * 주문서를 1개씩 추가하는 코루틴
     * -> updateTime마다 재귀호출 됨
     */

    IEnumerator OrderListUpdate(float updateTime)
    {
        yield return new WaitForSeconds(updateTime);

        AddOrderList(1);
        StartCoroutine(OrderListUpdate(updateTime));
    }


    /*
     * 주문서 num개의 주문서를 추가하는 함수
     * -> 랜덤으로 0 또는 1 숫자를 뽑아서 주문서 gameObject를 생성
     * -> gameObject를 _orderList에 _orderNumber 함께 Pair하여 추가
     * -> OrderObjectProgress 코루틴 실행
     */
    void AddOrderList(int num)
    {
        for (int i = 0; i < num; i++)
        {
            System.Random rand = new System.Random();
            int random = rand.Next(0, 2);

            GameObject orderObj;

            if (random == 0)
                orderObj = Managers.Resource.Instantiate("Fish_Order", null, null, _orderPrefabs.transform);
            else
                orderObj = Managers.Resource.Instantiate("Prawn_Order", null, null, _orderPrefabs.transform);

            _orderList.Add(new KeyValuePair<GameObject, int>(orderObj, ++_orderNumber));
            
            StartCoroutine(OrderObjectProgress(orderObj, _startPos + 70 * (_orderList.Count - 1), _orderNumber));
        }
    }


    /*
     * 주문서에 관한 애니메이션 코루틴
     * -> xPos만큼 x의 위치를 감소하여 주문서가 슬라이딩하는 애니메이션 연출
     * 
     * -> 주문서의 자식에서 progressBar를 찾아 바의 fillAmount를 관리
     *  -> 1초간 게이지바가 일정한 비율로 줄어들고, waitingAmount 값에 따라 색을 달리함
     *  
     * -> waitingAmount이 0 이하가 되면 (실패처리)
     *  -> OrderFail 함수 실행
     *  -> _orderList에서 삭제
     *  -> SortOrder 함수 실행
     *  -> OrderPrefabAnimation 코루틴 실행
     */
    IEnumerator OrderObjectProgress(GameObject obj, float xPos, int orderNumber)
    {
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-_startPos, 9); 

        while (obj.GetComponent<RectTransform>().anchoredPosition.x > xPos)
        {
            obj.GetComponent<RectTransform>().anchoredPosition -= new Vector2(_animationSpeed, 0);
            yield return null;
        }


        Transform progressBar = Define.FindDeepChild(obj.transform, "ProgressBar");
        float waitingAmount = progressBar.GetComponent<Image>().fillAmount;

        while (waitingAmount > 0 && obj != null)
        {
            waitingAmount -= 1 / _orderWaitingTime;
            progressBar.GetComponent<Image>().fillAmount = waitingAmount;

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
            else if(waitingAmount <= 0f)
                break;

            yield return new WaitForSeconds(1.0f);
        }

        int index = _orderList.FindIndex(x => x.Key.Equals(obj));

        if(waitingAmount <= 0)
        {
            OrderFail();
            _orderList.Remove(new KeyValuePair<GameObject, int>(obj, orderNumber));
            SortOrder(index, _orderList.Count);
            StartCoroutine(OrderPrefabAnimation(obj, "Fail", true));
        }
    }


    /* 
     * 플레이어가 반납한 음식을 판단하는 함수
     * -> 이벤트로 받아온 인자의 이름으로 판별
     *  -> (성공) _orderList를 모두 탐색하여 이름이 들어있으면
     *      -> OrderPrefabAnimation 코루틴 실행
     *      -> 성공했으므로 _orderList에서 제거
     *      -> Tag로 Passing(반납대)를 찾고 이펙트 생성 및 사운드 재생
     *      -> Score 증가
     *          -> text 변경, PlayerPrefs 값 변경
     *      -> SortOrder 함수 실행
     *  -> (실패) 이름이 없으면 
     *      -> OrderFail 함수 실행
     */
    void OrderListChecking(string foodName)
    {
        for (int i = 0; i < _orderList.Count; i++)
        {
            if (foodName == "Prawn" || foodName == "Fish")
            {
                if (_orderList[i].Key.name.Contains(foodName))
                {
                    StartCoroutine(OrderPrefabAnimation(_orderList[i].Key, "Success", true));
                    _orderList.Remove(_orderList[i]);

                    GameObject passing = GameObject.FindGameObjectWithTag("Passing");
                    Managers.Resource.Instantiate("OrderEffect", passing.transform.position + new Vector3(-1f, 1f, 0f), Quaternion.identity);
                    Managers.Sound.Play("Effect/UI/Order_Successful", Define.Sound.Effect);

                    _totalScore += _addScore;
                    _scoreText.text = _totalScore.ToString();
                    PlayerPrefs.SetInt("Success", ++_successFood);
                    
                    _orderCheck = true;

                    SortOrder(i, _orderList.Count);
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


    /*
     * 주문서를 정렬하는 함수
     * -> 인자로 받아온 num번째부터 max번째까지 _orderList에 있는 요소의 위치를 업데이트
     */
    void SortOrder(int num, int max)
    {
        for(int i = num; i < max; i++)
        {
            if (_orderList.Count <= i)
                continue;

            Vector2 temp = new Vector2(_startPos + 70 * i, 9);
            _orderList[i].Key.GetComponent<RectTransform>().anchoredPosition = temp;
        }
    }


    /*
     * 주문 실패시 호출되는 함수
     * -> Score 감소
     *  -> text 변경, PlayerPrefs 값 변경
     * -> 사운드 재생
     * -> _orderList 요소 개수만큼 OrderPrefabAnimation 코루틴 실행
     */
    void OrderFail()
    {
        _totalScore -= _minusScore;
        if (_totalScore < 0)
            _totalScore = 0;
        _scoreText.text = _totalScore.ToString();
        PlayerPrefs.SetInt("Fail", ++_failFood);

        Managers.Sound.Play("Effect/UI/Order_Fail", Define.Sound.Effect);

        for (int i = 0; i < _orderList.Count; i++)
        {
            StartCoroutine(OrderPrefabAnimation(_orderList[i].Key, "Fail", false));
        }
    }


    /*
     * 주문서 성공/실패, 파괴 가능/불가능에 따라 바뀌는 코루틴
     * -> result가 Success일 때
     *  -> gameObj의 "Success" gameObject를 킴
     *  -> 이미지의 색 변경
     * -> result가 Fail 일 때
     *  -> gameObj의 "Fail" gameObject를 킴
     *  -> 이미지의 색 변경
     * 
     * -> canDestroy가 true일 때
     *  -> gameObj 파괴
     * -> canDestroy가 false일 때
     *  -> 위에서 킨 gameObject를 끔
     */
    IEnumerator OrderPrefabAnimation(GameObject gameObj, string result, bool canDestroy)
    {
        Transform effectImage;
        float alpha = 0;
        Color changeColor;

        if (result == "Success")
            effectImage = Define.FindDeepChild(gameObj.transform, "Success");
        else
            effectImage = Define.FindDeepChild(gameObj.transform, "Fail");
        
        effectImage.gameObject.SetActive(true);

        while (alpha < 0.8f)
        {
            alpha += 0.2f;
            
            if(result == "Success")
                changeColor = new Color(0, 1, 0, alpha);
            else
                changeColor = new Color(1, 0, 0, alpha);
                
            effectImage.GetComponent<Image>().color = changeColor;
          
            yield return new WaitForSeconds(0.05f);
        }

        if(canDestroy)
            Managers.Resource.Destroy(gameObj);
        else
            effectImage.gameObject.SetActive(false);
    }

}
