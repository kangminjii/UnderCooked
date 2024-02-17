using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    //bool canInteract = false;
    //Transform _playerSpawnPos;
    Transform _binSpawnPos;
    AnimationClip animationClip;
    GameObject trash;
    public PassingGate passingGate;
    bool plateRemove = false;

    //public PlateReturn plateReturn;
    private void Start()
    {
        _binSpawnPos = transform;
       
    }

    private void Update()
    {

        if(_binSpawnPos.childCount > 0)
        {
            if (_binSpawnPos.childCount > 1)
                Destroy(trash);

            trash = _binSpawnPos.GetChild(0).gameObject;
            trash.GetComponent<Animator>().SetTrigger("binTrigger");
            


            if (trash.name.Contains("Plate") && !plateRemove)
            {
                GameObject Plate = trash;

                passingGate.plateReturn.PlateList.RemoveAt(passingGate.plateReturn.PlateList.Count - 1);
                passingGate.plateReturn.CurrentPlateNumber--;
                
                plateRemove = true;
            }

        }
            if (_binSpawnPos.childCount == 0)
                plateRemove = false;

    }
}
