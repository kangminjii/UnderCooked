using UnityEngine;
using UnityEngine.UI;

public class CookingPlace : MonoBehaviour
{
    string _prawnObjectName = "Prawn(Clone)";
    string _fishObjectName = "Fish(Clone)";

    public GameObject CookingKnife;
    public Transform SpawnPos;
    public Slider Slider;
    public GameObject OnFood;
    public float ChopCount = 0;
    public bool ChopObject = false;



    private void Update()
    {
        // 음식이 놓였는가?
        if (SpawnPos.childCount > 0)
        {
            CookingKnife.SetActive(false);

            OnFood = SpawnPos.GetChild(0).gameObject;

            if (OnFood.name == _prawnObjectName || OnFood.name == _fishObjectName)
            {
                ChopObject = true;
                Slider.gameObject.SetActive(true);
                Slider.value = ChopCount;
            }
            else
            {
                ChopObject = false;
                Slider.gameObject.SetActive(false);
            }
        }
        else
        {
            CookingKnife.SetActive(true);
            Slider.gameObject.SetActive(false);
        }
       
        // 다 썰렸는가?
        if (ChopCount >= 10)
        {
            string SliceObjectName = OnFood.name.Replace("(Clone)", "");

            Destroy(OnFood);
            Managers.Resource.Instantiate(SliceObjectName + "_Sliced", SpawnPos.position, Quaternion.Euler(0, -90, 0), SpawnPos);

            ChopObject = false;
            ChopCount = 0;
        }
    }


    public void CuttingFood()
    {
        ChopCount++;
    }

}