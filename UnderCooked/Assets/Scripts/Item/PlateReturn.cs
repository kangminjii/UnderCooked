using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturn : MonoBehaviour
{
    float _plateSpawnTime = 1.0f;
    int _maxPlateNumber = 3;
    string _plateName = "Plate";


    public int CurrentPlateNumber = 0;
    public Transform PlateSpawnPos;
    public List<GameObject> PlateList = new List<GameObject>();



    private void Update()
    {
        if (CurrentPlateNumber < _maxPlateNumber)
        {
            StartCoroutine(SpawnPlate());
        }
    }


    public IEnumerator SpawnPlate()
    {
        CurrentPlateNumber++;
       
        yield return new WaitForSeconds(_plateSpawnTime);

        StartPlate();
        
        if (CurrentPlateNumber < _maxPlateNumber)
            StartCoroutine(SpawnPlate());
    }


    public void StartPlate()
    {
        Vector3 spwanPlatePos = PlateSpawnPos.position + new Vector3(0, (PlateSpawnPos.childCount - 1) * 0.05f, 0);
        GameObject plate = Managers.Resource.Instantiate(_plateName, spwanPlatePos, Quaternion.identity, PlateSpawnPos);
        PlateList.Add(plate);

        Managers.Sound.Play("AudioClip/WashedPlate", Define.Sound.Effect);
    }

}
