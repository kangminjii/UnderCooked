using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingGate : MonoBehaviour
{
    Transform _playerSpawnPos;
    public bool canInteract = false;


    public PlateReturn plateReturn;


    void Start()
    {
        
    }

    private void Update()
    {
        //if (canInteract && Input.GetKeyDown(KeyCode.Space))
        //{
        //   if(_playerSpawnPos.childCount > 0 && _playerSpawnPos.GetChild(0).tag == "Plate")
        //   {
                
        //        plateReturn.PlateList.RemoveAt(plateReturn.CurrentPlateNumber - 1);
        //        plateReturn.CurrentPlateNumber--;

        //        StartCoroutine(plateReturn.SpawnPlate());
        //   }
           
        //}
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canInteract = true;
    //        _playerSpawnPos = other.transform.Find("SpawnPos");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canInteract = false;
    //    }
    //}
}
