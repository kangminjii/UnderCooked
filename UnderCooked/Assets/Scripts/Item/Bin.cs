using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    Transform _binSpawnPos;
    GameObject _trash;
    bool _plateRemove = false;

    public PlateReturn plateReturn;


    private void Start()
    {
        _binSpawnPos = transform;
    }

    private void Update()
    {
        if(_binSpawnPos.childCount > 0)
        {
            _trash = _binSpawnPos.GetChild(0).gameObject;
            _trash.GetComponent<Animator>().SetTrigger("binTrigger");
            
            if (_trash.name.Contains("Plate") && !_plateRemove)
            {
                plateReturn.PlateList.RemoveAt(plateReturn.PlateList.Count - 1);
                plateReturn.CurrentPlateNumber--;
                
                _plateRemove = true;
            }
        }

        if (_binSpawnPos.childCount == 0)
            _plateRemove = false;
    }
}
