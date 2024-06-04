using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    string[]    _foodIngredient = new string[2] { "Prawn(Clone)", "Fish(Clone)" };
    GameObject  _foodOnTable;
    [SerializeField]
    GameObject  _cookingKnife;
    [SerializeField]
    Transform   _objectSpawnPos;
    [SerializeField]
    Slider      _slider;


    [HideInInspector]
    public float ChopCount;
    [HideInInspector]
    public bool  CanChop;


    /*
     * Player�� Chop ��ų�� ����� �� �ִ� Table (����) - CookingPlace
     * -> ���۽� Object(CookingKnife, Slider) ��Ȱ��ȭ ����
     * -> Player�� Observer�� ��� �� ��ü�� ���� ��ȭ�� ���� ����
     */
    private void Awake()
    {
        _cookingKnife.SetActive(true);
        _slider.gameObject.SetActive(false);

        Player.Cooking += HandleCooking;
        Player.ChopCounting += HandleChopCounting;
    }
    

    /*
    * ������Ʈ ����� ���� ����
    */
    private void OnDestroy()
    {
        Player.Cooking -= HandleCooking;
        Player.ChopCounting -= HandleChopCounting;
    }


    /*
     * Player�� ������ Overlap�� �� �ҷ����� �Լ�
     * -> ������ ������ �������� child ������ �Ǵ�
     * 
     * -> ������ �ִٸ� Object(CookingKnife) ��Ȱ��ȭ
     * -> �� �� �ִ� ������� �̸����� �Ǻ���
     *      -> Chop ���� ���� ������Ʈ
     * 
     * -> ������ ���ٸ� Chop ���¿� ChopCount Ƚ�� �ʱ�ȭ
     */
    private void HandleCooking()
    {
        if (_objectSpawnPos.childCount > 0)
        {
            _cookingKnife.SetActive(false);
            _foodOnTable = _objectSpawnPos.GetChild(0).gameObject;

            for (int i = 0; i < _foodIngredient.Length; i++)
            {
                if(_foodOnTable.name == _foodIngredient[i])
                {
                    CanChop = true;
                    _slider.gameObject.SetActive(true);
                    break;
                }
                
                CanChop = false;
            }

            return;
        }

        CanChop = false;
        ChopCount = 0;
        _slider.value = ChopCount;

        _cookingKnife.SetActive(true);
        _slider.gameObject.SetActive(false);
    }


    /*
     * Player�� Chop Animation�� ��ġ�� ȣ��Ǵ� �Լ�
     * -> ChopCount Ƚ�� ���� �� Slider �� ����
     * -> ����Ʈ ���� �� ���� ���
     * 
     * -> ChopCount�� 10ȸ�� �Ǹ� �ʱ�ȭ
     * -> ��� �ı� �� �ϼ��� �������� ����
     */
    public void HandleChopCounting()
    {
        if (ChopCount < 10)
        {
            ChopCount++;
            _slider.value = ChopCount;
        }
        else
        {
            CanChop = false;
            ChopCount = 0;
            _slider.value = ChopCount;
            _slider.gameObject.SetActive(false);

            string slicedObjectName = _foodOnTable.name.Replace("(Clone)", "");

            Destroy(_foodOnTable);
            
            Managers.Resource.Instantiate(slicedObjectName + "_Sliced", _objectSpawnPos.position, Quaternion.Euler(0, -90, 0), _objectSpawnPos);
        }
    }
   
}