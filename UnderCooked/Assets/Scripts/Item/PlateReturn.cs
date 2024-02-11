using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturn : MonoBehaviour
{
    // :: private
    float _plateSpawnTime = 1.0f;
    int _maxPlateNumber = 2;
    string _plateName = "Plate";
    bool canInteract = false;

    public Transform PlateSpawnPos;
    Transform _playerSpawnPos;


    // :: public
    public int CurrentPlateNumber = 0;
    public List<GameObject> PlateList = new List<GameObject>();



    private void Start()
    {
        PlateSpawnPos = this.transform.Find("PlateSpawnPos");
        StartCoroutine(SpawnPlate());
    }

    private void Update()
    {
        //if (canInteract && Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(CurrentPlateNumber > 0 && _playerSpawnPos.childCount < 1)
        //    {
        //        Managers.Resource.Instantiate(_plateName, _playerSpawnPos.position, Quaternion.identity, _playerSpawnPos.transform);
        //        Managers.Resource.Destroy(PlateSpawnPos.GetChild(PlateSpawnPos.childCount-1).gameObject);
        //    }
        //}
        //Debug.Log(CurrentPlateNumber);
        if (CurrentPlateNumber < _maxPlateNumber)
        {
            StartCoroutine(SpawnPlate());
            Debug.Log("업데이트");
        }


    }


    public IEnumerator SpawnPlate()
    {
        CurrentPlateNumber++;

        
        yield return new WaitForSeconds(_plateSpawnTime);

        Vector3 spwanPlatePos = PlateSpawnPos.position + new Vector3(0, (PlateSpawnPos.childCount - 1) * 0.05f, 0);
        GameObject plate = Managers.Resource.Instantiate(_plateName, spwanPlatePos, Quaternion.identity, PlateSpawnPos);
        PlateList.Add(plate);
        Debug.Log("코루틴");
        if (CurrentPlateNumber < _maxPlateNumber)
            StartCoroutine(SpawnPlate());
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
