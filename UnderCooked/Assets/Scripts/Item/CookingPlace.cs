using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private GameObject _cookingKnife;
    private Transform _spawnPos;
    private GameObject _onFood;

    public int _chopCount = 0;

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
            _onFood = _spawnPos.transform.GetChild(0).gameObject;
           // Food_Enter(this.transform.GetChild(0).gameObject);
        }
       else
            _cookingKnife.SetActive(true);

       if(_chopCount >= 10)
        {
            Destroy(_onFood);
            _chopCount = 0;
        }
    }

    public void CuttingFood()
    {
        this._chopCount++;
    }    

}
