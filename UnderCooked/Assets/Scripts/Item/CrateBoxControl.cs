using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    Animator _animtor;
    Transform _spawnPoint;

    KeyCode triggerKey = KeyCode.Space;

    bool canInteract = false;
    string _foodName;


    private void Start()
    {
        _animtor = GetComponent<Animator>();
        _foodName = this.transform.parent.name;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(triggerKey))
        {
            _animtor.SetTrigger("IsOpen");
            SpawnObj(_foodName);
        }
    }


    public void SpawnObj(string name)
    {
        string boxName = "Crate_";
        name = name.Remove(0, boxName.Length);

        Managers.Resource.Instantiate(name, _spawnPoint.position, Quaternion.identity, _spawnPoint.transform);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            _spawnPoint = other.transform.Find("SpawnPos");
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
