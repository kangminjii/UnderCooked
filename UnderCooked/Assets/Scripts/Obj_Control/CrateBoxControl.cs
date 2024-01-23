using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
  private KeyCode triggerKey = KeyCode.Space;
    
    Animator _animtor;
    GameObject _playerObject;
    Transform _spawnPoint;
    bool canInteract = false;


    private void Start()
    {
        _animtor = GetComponent<Animator>();
        _playerObject = GameObject.Find("Chef");
        _spawnPoint = _playerObject.transform.Find("SpawnPoint");
    }
    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (Managers.Instance.IsGrab == false)
            {
                _animtor.SetTrigger("IsOpen");
                SpawnObj();
            }
        }
    }


    public void SpawnObj()
    {
        GameObject instance = Managers.Resource.Instantiate("Prawn", _spawnPoint.position, Quaternion.identity, _playerObject.transform);
        Managers.Resource.PlayerGrabItem.Add(instance);

        Managers.Instance.IsGrab = true;
        Managers.Instance.IsDrop = false;
        Invoke("SetIsPickPrawnTrue", 0.3f);
    }

    public void SetIsPickPrawnTrue()
    {
        Managers.Instance.IsPick_Prawn = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        // 특정 조건을 검사하여 충돌을 무시
        if (collision.gameObject.CompareTag("Food"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }
}
