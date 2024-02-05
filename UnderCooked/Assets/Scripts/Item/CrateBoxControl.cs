using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    // : private
    Animator _animator;
    Transform _spawnPoint;
    //GameObject _playerSpawnPos;

    bool canInteract = true; // �ʱ⿡�� ��ȣ �ۿ� ����
    string _foodName;

    float spawnDelay = 1.5f; // ���� ���� �ð�

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _foodName = this.transform.parent.parent.name;
    }

    private void Update()
    {

        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {

            if (_spawnPoint != null && _spawnPoint.transform.childCount < 1)
            {
                _animator.SetTrigger("IsOpen");
                SpawnObj(_foodName);
                canInteract = false; // ���� �Ŀ� ��ȣ �ۿ� ��Ȱ��ȭ
                StartCoroutine(EnableInteractAfterDelay());
            }
        }
    }

    IEnumerator EnableInteractAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        canInteract = true; // ���� �ð��� ���� �Ŀ� �ٽ� ��ȣ �ۿ� �����ϰ� ����
    }

    public void SpawnObj(string name)
    {
        string boxName = "Crate_";
        name = name.Remove(0, boxName.Length);

        if (name == "Fish")
        {
            Vector3 newPosition = _spawnPoint.position + new Vector3(0f, 0.3f, 0f); // y���� 1��ŭ �ø�
            Managers.Resource.Instantiate(name, newPosition, Quaternion.identity, _spawnPoint.transform);
        }
        else
            Managers.Resource.Instantiate(name, _spawnPoint.position, Quaternion.identity, _spawnPoint.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spawnPoint = other.transform.Find("SpawnPos");
           // _playerSpawnPos = _spawnPoint.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _spawnPoint = null;
    }


}