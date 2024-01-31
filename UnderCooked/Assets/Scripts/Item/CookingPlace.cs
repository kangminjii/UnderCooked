using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private GameObject _cookingKnife;
    private Transform _spawnPos;

    [SerializeField] int ChopCount = 0;

    public delegate void Chop_Food(GameObject gameObject);
    public static event Chop_Food Food_Enter;
    //public static event Chop_Food Food_Exit;


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
            Food_Enter(this.transform.GetChild(0).gameObject);
        }
       else
            _cookingKnife.SetActive(true);

       if(ChopCount >= 10)
        {
            Food_Enter(null);
            ChopCount = 0;
        }
    }

    public void CuttingFood()
    {
        ChopCount++;
    }    

}
