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
     * Player가 Chop 스킬을 사용할 수 있는 Table (도마) - CookingPlace
     * -> 시작시 Object(CookingKnife, Slider) 비활성화 상태
     * -> Player를 Observer로 등록 후 주체의 상태 변화에 따라 갱신
     */
    private void Awake()
    {
        _cookingKnife.SetActive(true);
        _slider.gameObject.SetActive(false);

        Player.Cooking += HandleCooking;
        Player.ChopCounting += HandleChopCounting;
    }
    

    /*
    * 프로젝트 종료시 구독 해제
    */
    private void OnDestroy()
    {
        Player.Cooking -= HandleCooking;
        Player.ChopCounting -= HandleChopCounting;
    }


    /*
     * Player가 도마를 Overlap할 때 불러지는 함수
     * -> 도마에 음식이 놓였는지 child 개수로 판단
     * 
     * -> 음식이 있다면 Object(CookingKnife) 비활성화
     * -> 썰 수 있는 재료인지 이름으로 판별함
     *      -> Chop 가능 상태 업데이트
     * 
     * -> 음식이 없다면 Chop 상태와 ChopCount 횟수 초기화
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
     * Player가 Chop Animation을 마치면 호출되는 함수
     * -> ChopCount 횟수 증가 및 Slider 값 변경
     * -> 이펙트 생성 및 사운드 출력
     * 
     * -> ChopCount가 10회가 되면 초기화
     * -> 재료 파괴 후 완성된 음식으로 생성
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