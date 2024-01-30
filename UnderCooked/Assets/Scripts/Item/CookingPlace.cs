using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private GameObject _cookingKnife;
    private Transform _spawnPos;


    private void Start()
    {
        _cookingKnife =this.transform.Find("CuttingBoard_Knife").gameObject;
        _spawnPos = this.transform.Find("SpawnPos");
    }

    private void Update()
    {
       if(_spawnPos.childCount > 0)
            _cookingKnife.SetActive(false);
       else
            _cookingKnife.SetActive(true);
    }
}
