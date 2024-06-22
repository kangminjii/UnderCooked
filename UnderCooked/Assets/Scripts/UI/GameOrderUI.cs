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
     * ���� �� ����
     * -> GameSceneUI���� ������ ���۵� �� OrderStart �Լ� ����
     * -> ������ ������ �� OrderListChecking �Լ� ����
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
     * ������ ���۵� �� ȣ��Ǵ� �Լ�
     * -> PlayerPrefs �ʱ�ȭ
     * -> �ֹ��� 2�� ����
     * -> _updateTime���� ȣ��Ǵ� ��� �ڷ�ƾ ����
     */
    void OrderStart()
    {
        PlayerPrefs.DeleteAll();
        AddOrderList(2);
        StartCoroutine(OrderListUpdate(_updateTime));
    }


    /*
     * �ֹ����� 1���� �߰��ϴ� �ڷ�ƾ
     * -> updateTime���� ���ȣ�� ��
     */

    IEnumerator OrderListUpdate(float updateTime)
    {
        yield return new WaitForSeconds(updateTime);

        AddOrderList(1);
        StartCoroutine(OrderListUpdate(updateTime));
    }


    /*
     * �ֹ��� num���� �ֹ����� �߰��ϴ� �Լ�
     * -> �������� 0 �Ǵ� 1 ���ڸ� �̾Ƽ� �ֹ��� gameObject�� ����
     * -> gameObject�� _orderList�� _orderNumber �Բ� Pair�Ͽ� �߰�
     * -> OrderObjectProgress �ڷ�ƾ ����
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
     * �ֹ����� ���� �ִϸ��̼� �ڷ�ƾ
     * -> xPos��ŭ x�� ��ġ�� �����Ͽ� �ֹ����� �����̵��ϴ� �ִϸ��̼� ����
     * 
     * -> �ֹ����� �ڽĿ��� progressBar�� ã�� ���� fillAmount�� ����
     *  -> 1�ʰ� �������ٰ� ������ ������ �پ���, waitingAmount ���� ���� ���� �޸���
     *  
     * -> waitingAmount�� 0 ���ϰ� �Ǹ� (����ó��)
     *  -> OrderFail �Լ� ����
     *  -> _orderList���� ����
     *  -> SortOrder �Լ� ����
     *  -> OrderPrefabAnimation �ڷ�ƾ ����
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
     * �÷��̾ �ݳ��� ������ �Ǵ��ϴ� �Լ�
     * -> �̺�Ʈ�� �޾ƿ� ������ �̸����� �Ǻ�
     *  -> (����) _orderList�� ��� Ž���Ͽ� �̸��� ���������
     *      -> OrderPrefabAnimation �ڷ�ƾ ����
     *      -> ���������Ƿ� _orderList���� ����
     *      -> Tag�� Passing(�ݳ���)�� ã�� ����Ʈ ���� �� ���� ���
     *      -> Score ����
     *          -> text ����, PlayerPrefs �� ����
     *      -> SortOrder �Լ� ����
     *  -> (����) �̸��� ������ 
     *      -> OrderFail �Լ� ����
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
     * �ֹ����� �����ϴ� �Լ�
     * -> ���ڷ� �޾ƿ� num��°���� max��°���� _orderList�� �ִ� ����� ��ġ�� ������Ʈ
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
     * �ֹ� ���н� ȣ��Ǵ� �Լ�
     * -> Score ����
     *  -> text ����, PlayerPrefs �� ����
     * -> ���� ���
     * -> _orderList ��� ������ŭ OrderPrefabAnimation �ڷ�ƾ ����
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
     * �ֹ��� ����/����, �ı� ����/�Ұ��ɿ� ���� �ٲ�� �ڷ�ƾ
     * -> result�� Success�� ��
     *  -> gameObj�� "Success" gameObject�� Ŵ
     *  -> �̹����� �� ����
     * -> result�� Fail �� ��
     *  -> gameObj�� "Fail" gameObject�� Ŵ
     *  -> �̹����� �� ����
     * 
     * -> canDestroy�� true�� ��
     *  -> gameObj �ı�
     * -> canDestroy�� false�� ��
     *  -> ������ Ų gameObject�� ��
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
