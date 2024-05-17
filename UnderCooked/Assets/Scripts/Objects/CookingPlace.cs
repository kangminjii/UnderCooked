using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    string _prawnObjectName = "Prawn(Clone)";
    string _fishObjectName = "Fish(Clone)";
    Player _player;


    public GameObject   CookingKnife;
    public GameObject   OnFood;
    public Transform      SpawnPos;
    public Slider            Slider;
    public float             ChopCount = 0;
    public bool             CanChop = false;


    /*
     * Player�� Chop ��ų�� ����� �� �ִ� Table (����)
     * -> ���۽� Prefab Object ��Ȱ��ȭ ����
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
        // ������ �����°�?
        if (SpawnPos.childCount > 0)
        {
            Debug.Log("������ ������");

            CookingKnife.SetActive(false);

            OnFood = SpawnPos.GetChild(0).gameObject;

            if (OnFood.name == _prawnObjectName || OnFood.name == _fishObjectName)
            {
                CanChop = true;
                Slider.gameObject.SetActive(true);
                Slider.value = ChopCount;
            }
            else
            {
                CanChop = false;
                Slider.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("������ �ȳ�����");

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

        Managers.Resource.Instantiate("Chophit", _player.ChopPos.position, Quaternion.identity, _player.ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
    }
   
}