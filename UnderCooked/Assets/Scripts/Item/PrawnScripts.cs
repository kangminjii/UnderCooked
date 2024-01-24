using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrawnScripts : MonoBehaviour
{
    public int Count = 10;
    private GameObject _playerObject;
    private Transform _spawnPoint;
    public GameObject _selectPrawn;
    private bool Ok;
    


    private void Start()
    {
        _playerObject = GameObject.Find("Chef");
        _spawnPoint = _playerObject.transform.Find("SpawnPoint");
    }

    void Update()
    {
        if (Ok)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Managers.Instance.IsGrab && Managers.Instance.IsCan_Pick)
            {
                Debug.Log("Space key pressed, calling PickSpawn");
                PickSpawn();
                Managers.Instance.IsCan_Pick = false;
            }
        }
    }



    private void PickSpawn()
    {
        GameObject instance = Managers.Resource.Instantiate("Prawn", _spawnPoint.position, Quaternion.identity, _spawnPoint);
        Managers.Resource.PlayerGrabItem.Add(instance);


        Managers.Instance.IsGrab = true;
        Managers.Instance.SetPrawnBool();
        Destroy(_selectPrawn);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {

            _selectPrawn = this.gameObject;
            Debug.Log("true");
            Debug.Log(_selectPrawn);
            Ok = true;       
        }

        if (other.CompareTag("Plane"))
        {
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            //MeshCollider mesh = this.gameObject.GetComponent<MeshCollider>();

            if (meshCollider != null)
            {
                meshCollider.convex = true;
            }
        }

    }




    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ok = false;
            Debug.Log("false");
        }
    }

}
