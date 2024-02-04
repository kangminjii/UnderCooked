using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    private GameObject _cookingKnife;
    private Transform _spawnPos;
    public GameObject OnFood;


    //public Slider CutSlider;
    

    string PrawnObjectName = "Prawn(Clone)";
    string FishObjectName = "Fish(Clone)";

    public int _chopCount = 0;
    public bool SliceFoodbool = false;



    private void Start()
    {
        _cookingKnife = transform.Find("CuttingBoard_Knife").gameObject;
        _spawnPos = transform.Find("SpawnPos");
       // CutSlider = GameObject.Find("Canvas").transform.Find("Slider").GetComponent<Slider>();


        //CutSlider.minValue = 0;
        //CutSlider.maxValue = 10;
       


    }

    private void Update()
    {

        //CutSlider.value = _chopCount;

        //if (CutSlider.value == 0)
        //{
        //    CutSlider.gameObject.SetActive(false);
        //}
        //else
        //    CutSlider.gameObject.SetActive(true);



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

        if (_chopCount >= 10)
        {
            
            //onFood slicefood 설정안되는거 수정해야함
            string clone = "(Clone)";
            string SliceObjectName = _spawnPos.GetChild(0).name;
            SliceObjectName = SliceObjectName.Replace(clone, "");

            Destroy(OnFood);

            Managers.Resource.Instantiate(SliceObjectName + "_Sliced", _spawnPos.position, Quaternion.Euler(0, -90, 0), _spawnPos);

            SliceFoodbool = true;
            _chopCount = 0;
            //CutSlider.value = 0;

        }

        

    }

    public void CuttingFood()
    {
        _chopCount++;
    }

}