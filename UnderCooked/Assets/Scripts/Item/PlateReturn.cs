using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturn : MonoBehaviour
{
    // :: private
    float _plateSpawnTime = 1.0f;
    int _maxPlateNumber = 2;
    string _plateName = "Plate";

    public Transform PlateSpawnPos;

    // :: public
    public int CurrentPlateNumber = 0;
    public List<GameObject> PlateList = new List<GameObject>();



    private void Start()
    {
        PlateSpawnPos = this.transform.Find("PlateSpawnPos");
    }


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

        Vector3 spwanPlatePos = PlateSpawnPos.position + new Vector3(0, (PlateSpawnPos.childCount - 1) * 0.05f, 0);
        GameObject plate = Managers.Resource.Instantiate(_plateName, spwanPlatePos, Quaternion.identity, PlateSpawnPos);
        PlateList.Add(plate);
        if (CurrentPlateNumber < _maxPlateNumber)
            StartCoroutine(SpawnPlate());
    }


}
