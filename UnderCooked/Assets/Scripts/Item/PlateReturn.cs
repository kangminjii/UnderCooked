using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturn : MonoBehaviour
{

    public int PlateNumber;


    float _spawnTime = 1.0f;
    int _maxNumber = 2;
    string _plateName = "Plate";
    Transform _spawnPos;
    Transform _playerSpawnPoint;
    KeyCode triggerKey = KeyCode.Space;
    bool canInteract = false;
    
    List<GameObject> PlateList = new List<GameObject>();


    private void Start()
    {
        _spawnPos = this.transform.Find("SpawnPos");
        StartCoroutine(SpawnPlate());
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(triggerKey))
        {
            if(PlateNumber > 0)
            {
                Managers.Resource.Instantiate(_plateName, _playerSpawnPoint.position, Quaternion.identity, _playerSpawnPoint.transform);
                Managers.Resource.Destroy(PlateList[PlateNumber - 1]);
                PlateList.RemoveAt(PlateNumber - 1);
                PlateNumber--;
            }
        }
    }

    IEnumerator SpawnPlate()
    {
        PlateNumber++;

        yield return new WaitForSeconds(_spawnTime);

        GameObject plate = Managers.Resource.Instantiate(_plateName, _spawnPos.position + new Vector3(0, (PlateNumber-1) * 0.05f, 0), Quaternion.identity);
        PlateList.Add(plate);

        if (PlateNumber < _maxNumber)
            StartCoroutine(SpawnPlate());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            _playerSpawnPoint = other.transform.Find("SpawnPos");
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
