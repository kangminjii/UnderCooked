using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    string[] _foodIngredient = new string[2] { "Prawn(Clone)", "Fish(Clone)" };
    Player   _player;


    public GameObject   CookingKnife;
    public GameObject   FoodAtTable;
    public Transform    SpawnPos;
    public Slider       Slider;
    public float        ChopCount;
    public bool         CanChop;


    /*
     * Player�� Chop ��ų�� ����� �� �ִ� Table (����) - CookingPlace
     * -> ���۽� Object(CookingKnife, Slider) ��Ȱ��ȭ ����
     * -> Player�� Observer�� ��� �� HandleCooking() �Լ� ����
     */
    private void Awake()
    {
        CookingKnife.SetActive(true);
        Slider.gameObject.SetActive(false);

        _player = FindObjectOfType<Player>();

        if (_player != null)
        {
            _player.Cooking += HandleCooking;
        }
    }
    

    /*
    * ������Ʈ ����� ���� ����
    */
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.Cooking -= HandleCooking;
        }
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
            FoodAtTable = SpawnPos.GetChild(0).gameObject;

            for (int i = 0; i < _foodIngredient.Length; i++)
            {
                if(FoodAtTable.name == _foodIngredient[i])
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

            Managers.Resource.Instantiate("Chophit", _player.ChopPos.position, Quaternion.identity, _player.ChopPos);
            Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
        }
        else
        {
            CanChop = false;
            ChopCount = 0;
            Slider.value = ChopCount;
            Slider.gameObject.SetActive(false);

            string slicedObjectName = FoodAtTable.name.Replace("(Clone)", "");

            Destroy(FoodAtTable);
            
            Managers.Resource.Instantiate(slicedObjectName + "_Sliced", SpawnPos.position, Quaternion.Euler(0, -90, 0), SpawnPos);
        }
    }
   
}