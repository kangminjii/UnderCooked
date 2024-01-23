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
        if (canInteract && Input.GetKeyDown(triggerKey))
        {
            _animtor.SetTrigger("IsOpen");
            canInteract = false;
            if (Managers.Instance.IsGrab == false)
            {
                SpawnObj();
            }
        }
    }


    public void SpawnObj()
    {
        GameObject instance = Managers.Resource.Instantiate("Prawn", _spawnPoint.position, Quaternion.identity, _playerObject.transform);
        Debug.Log("instance: " + instance);
        Managers.Resource.PlayerPrawn.Add(instance);

        Managers.Instance.IsGrab = true;
        Managers.Instance.IsDrop = false;
        Invoke("SetIsPickPrawnTrue", 0.3f);
    }

    public void SetIsPickPrawnTrue()
    {
        Managers.Instance.IsPick_Prawn = true;
    }

    private void OnTriggerEnter(Collider other)
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
        // Ư�� ������ �˻��Ͽ� �浹�� ����
        if (collision.gameObject.CompareTag("Food"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }
}
