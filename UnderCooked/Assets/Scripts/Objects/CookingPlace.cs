using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    [SerializeField]
    GameObject  CookingKnife;
    [SerializeField]
    Transform   SpawnPos;
    [SerializeField]
    Slider      Slider;
    
    string[]    _foodIngredient = new string[2] { "Prawn(Clone)", "Fish(Clone)" };
    GameObject  _foodOnTable;
    

    public float ChopCount;
    public bool  CanChop;


    /*
     * Player�� Chop ��ų�� ����� �� �ִ� Table (����) - CookingPlace
     * -> ���۽� Object(CookingKnife, Slider) ��Ȱ��ȭ ����
     * -> Player�� Observer�� ��� �� HandleCooking(), HandleChopCounting() �Լ� ����
     */
    private void Awake()
    {
        CookingKnife.SetActive(true);
        Slider.gameObject.SetActive(false);

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
        if (SpawnPos.childCount > 0)
        {
            CookingKnife.SetActive(false);
            _foodOnTable = SpawnPos.GetChild(0).gameObject;

            for (int i = 0; i < _foodIngredient.Length; i++)
            {
                if(_foodOnTable.name == _foodIngredient[i])
                {
                    CanChop = true;
                    Slider.gameObject.SetActive(true);
                    break;
                }
                
                CanChop = false;
            }

            return;
        }

        CanChop = false;
        ChopCount = 0;
        Slider.value = ChopCount;

        CookingKnife.SetActive(true);
        Slider.gameObject.SetActive(false);
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
            Slider.value = ChopCount;
        }
        else
        {
            CanChop = false;
            ChopCount = 0;
            Slider.value = ChopCount;
            Slider.gameObject.SetActive(false);

            string slicedObjectName = _foodOnTable.name.Replace("(Clone)", "");

            Destroy(_foodOnTable);
            
            Managers.Resource.Instantiate(slicedObjectName + "_Sliced", SpawnPos.position, Quaternion.Euler(0, -90, 0), SpawnPos);
        }
    }
   
}