using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    private GameObject _cookingKnife;
    private Transform _spawnPos;
    public Slider _slider;


    public GameObject OnFood;

    

    string PrawnObjectName = "Prawn(Clone)";
    string FishObjectName = "Fish(Clone)";

    public int _chopCount = 0;
    public bool SliceFoodbool = false;



    private void Start()
    {
        _cookingKnife = transform.Find("CuttingBoard_Knife").gameObject;
        _spawnPos = transform.Find("SpawnPos");
        _slider = GetComponentInChildren<Slider>();

        _slider.minValue = 0;
        _slider.maxValue = 10;
    }


    private void Update()
    {
        if (_spawnPos.childCount > 0)
        {
            _cookingKnife.SetActive(false);

            OnFood = _spawnPos.transform.GetChild(0).gameObject;

            if (OnFood.name == PrawnObjectName)
            {
                SliceFoodbool = false;
            }
            else if (OnFood.name == FishObjectName)
            {
                SliceFoodbool = false;
            }
            else SliceFoodbool = true;
        }
        else
            _cookingKnife.SetActive(true);

        if (OnFood != null && !SliceFoodbool)
        {
            _slider.gameObject.SetActive(true);
            _slider.value = (float)_chopCount;
        }
        else _slider.gameObject.SetActive(false);

        if (_chopCount >= 10)
        {
            string clone = "(Clone)";
            string SliceObjectName = _spawnPos.GetChild(0).name;
            SliceObjectName = SliceObjectName.Replace(clone, "");

            Destroy(OnFood);

            Managers.Resource.Instantiate(SliceObjectName + "_Sliced", _spawnPos.position, Quaternion.Euler(0, -90, 0), _spawnPos);

            SliceFoodbool = true;
            _chopCount = 0;
        }
    }


    public void CuttingFood()
    {
        _chopCount++;
    }

}