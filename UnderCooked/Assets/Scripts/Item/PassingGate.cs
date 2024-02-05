using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingGate : MonoBehaviour
{

    bool canInteract = false;
    Transform _playerSpawnPos;


    public PlateReturn plateReturn;
    public static Action FoodOrderCheck;

    void Start()
    {

    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {
           if(_playerSpawnPos.childCount > 0)
           {
                plateReturn.PlateList.RemoveAt(plateReturn.CurrentPlateNumber - 1);
                plateReturn.CurrentPlateNumber--;

                StartCoroutine(plateReturn.SpawnPlate());

                FoodOrderCheck.Invoke();
           }
           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            _playerSpawnPos = other.transform.Find("SpawnPos");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
