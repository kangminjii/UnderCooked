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
     * Player가 Chop 스킬을 사용할 수 있는 Table (도마)
     * -> 시작시 Prefab Object 비활성화 상태
     * -> Player를 Observer로 등록 후 HandleCooking() 함수 구독
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
    * 프로젝트 종료시 구독 해제
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
     * Player가 도마를 Overlap할 때 불러지는 함수
     * -> 도마에 음식이 놓였는지 child 개수로 판단
     * -> 
     */
    private void HandleCooking()
    {
        // 음식이 놓였는가?
        if (SpawnPos.childCount > 0)
        {
            Debug.Log("음식이 놓였다");

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
            Debug.Log("음식이 안놓였다");

            CookingKnife.SetActive(true);
            Slider.gameObject.SetActive(false);
        }
       
        // 다 썰렸는가?
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
     * Player가 Chop Animation을 마치면 호출되는 함수
     * -> ChopCount 횟수를 증가
     * -> 이펙트 생성 및 사운드 출력
     */
    public void HandleChopCounting()
    {
        ChopCount++;

        Managers.Resource.Instantiate("Chophit", _player.ChopPos.position, Quaternion.identity, _player.ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
    }
   
}