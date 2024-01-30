using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    Animator _animtor;
    GameObject _playerObject;
    Transform _spawnPoint;

    KeyCode triggerKey = KeyCode.Space;
    bool canInteract = false;


    private void Start()
    {
        _animtor = GetComponent<Animator>();
        _playerObject = GameObject.Find("Chef");
        _spawnPoint = _playerObject.transform.Find("SpawnPoint");
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyUp(triggerKey))
        {
            _animtor.SetTrigger("IsOpen");
            SpawnObj();
        }
    }


    public void SpawnObj()
    {
        GameObject instance = Managers.Resource.Instantiate("Prawn", _spawnPoint.position, Quaternion.identity, _playerObject.transform);
        Managers.Resource.PlayerGrabItem.Add(instance);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
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
