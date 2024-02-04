using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private GameObject _cookingKnife;
    private Transform _spawnPos;
    public GameObject OnFood;
    public GameObject SliceFood;
    

    public int _chopCount = 0;
    public bool SliceFoodbool = false;

    //public delegate void Chop_Food(GameObject gameObject);
    //public static event Chop_Food Food_Enter;
    ////public static event Chop_Food Food_Exit;


    private void Start()
    {
        _cookingKnife = transform.Find("CuttingBoard_Knife").gameObject;
        _spawnPos = this.transform.Find("SpawnPos");
    }

    private void Update()
    {
       if(_spawnPos.childCount > 0)
        {
            _cookingKnife.SetActive(false);
            OnFood = _spawnPos.transform.GetChild(0).gameObject;
          
        }
       else
            _cookingKnife.SetActive(true);

        if (_chopCount >= 10)
        {
            //onFood slicefood 설정안되는거 수정해야함
            string clone = "(Clone)";
            string SliceObjectName = _spawnPos.GetChild(0).name;
            SliceObjectName = SliceObjectName.Replace(clone, "");


            Managers.Resource.Instantiate(SliceObjectName + "_Sliced", _spawnPos.position, Quaternion.Euler(0, -90, 0), _spawnPos);
            
            SliceFood = _spawnPos.transform.GetChild(0).gameObject;
            SliceFoodbool = true;

            
            _chopCount = 0;

        }

        if(SliceFood != null)
        {
            SliceFoodbool = false;
        }

    }

    public void CuttingFood()
    {
        _chopCount++;
    }    

}
