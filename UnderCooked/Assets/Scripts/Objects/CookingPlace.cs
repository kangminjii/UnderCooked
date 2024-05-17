using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    private string[] _foodIngredient = new string[2] { "Prawn(Clone)", "Fish(Clone)" };
    private Player   _player;


    public GameObject CookingKnife;
    public GameObject OnFood;
    public Transform  SpawnPos;
    public Slider     Slider;
    public float      ChopCount = 0;
    public bool       CanChop = false;


    /*
     * Player�� Chop ��ų�� ����� �� �ִ� Table (����) - CookingPlace
     * -> ���۽� Object(CookingKnife, Slider) ��Ȱ��ȭ ����
     * -> Player�� Observer�� ��� �� HandleCooking() �Լ� ����
     */
    private void Start()
    {
        CookingKnife.SetActive(true);
        Slider.gameObject.SetActive(false);

        _player = FindObjectOfType<Player>();
        
        if (_player != null)
        {
            _player.Cooking += HandleCooking;
            _player.ChopCounting += HandleChopCounting;
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
            _player.ChopCounting -= HandleChopCounting;
        }
    }


    /*
     * Player�� ������ Overlap�� �� �ҷ����� �Լ�
     * -> ������ ������ �������� child ������ �Ǵ�
     * -> 
     */
    private void HandleCooking()
    {
        if (SpawnPos.childCount > 0)
        {
            // ������ ����
            CookingKnife.SetActive(false);

            OnFood = SpawnPos.GetChild(0).gameObject;

            for(int i = 0; i < _foodIngredient.Length; i++)
            {
                if(OnFood.name == _foodIngredient[i])
                {
                    CanChop = true;
                    Slider.gameObject.SetActive(true);
                    break;
                }
                
                CanChop = false;
                Slider.gameObject.SetActive(false);
            }
        }
        else
        {
            // ������ �ȳ���
            CookingKnife.SetActive(true);
            Slider.gameObject.SetActive(false);
        }
       

        // �� ��ȴ°�?
        if (ChopCount >= 10)
        {
            string SliceObjectName = OnFood.name.Replace("(Clone)", "");

            Destroy(OnFood);
            Managers.Resource.Instantiate(SliceObjectName + "_Sliced", SpawnPos.position, Quaternion.Euler(0, -90, 0), SpawnPos);

            CanChop = false;
            ChopCount = 0;
        }
    }


    /*
     * Player�� Chop Animation�� ��ġ�� ȣ��Ǵ� �Լ�
     * -> ChopCount Ƚ���� ����
     * -> ����Ʈ ���� �� ���� ���
     */
    public void HandleChopCounting()
    {
        ChopCount++;
        Slider.value = ChopCount;

        Managers.Resource.Instantiate("Chophit", _player.ChopPos.position, Quaternion.identity, _player.ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
       
    }
   
}