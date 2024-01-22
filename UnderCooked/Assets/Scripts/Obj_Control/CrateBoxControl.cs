using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    public string animName = "CrateBox";
    public KeyCode triggerKey = KeyCode.LeftControl;
    public GameObject Prawn;
    public Transform spawnPoint;
    public GameObject light;

    private Player player;

    public Animator animtor;

    bool canInteract = false;

    private void Start()
    {
        animtor = GetComponent<Animator>();
        //GameObject instance = Instantiate(Prawn, spawnPoint.position, Quaternion.identity);

        //instance.transform.SetParent(spawnPoint);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("tt");
            light.SetActive(true);
            canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("tt");
            light.SetActive(false);
            canInteract = false;
        }
    }

    private void PlayAnimation()
    {
        animtor.SetTrigger("IsOpen");
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(triggerKey))
        {

            PlayAnimation();
            canInteract = false;
            if(Managers.Instance.IsGrab == false)
            {
                SpawnObj();
                
            }
        }

    }

    public void SpawnObj()
    {

        //GameObject Instance = Instantiate(Prawn);

        //spawnPoint.position = player.leftHand.transform.position;

        //Debug.Log(spawnPoint.position);

        //GameObject instance = Instantiate(Prawn, spawnPoint.position, Quaternion.identity);
        //instance.transform.parent = player.transform;

        GameObject instance = Instantiate(Prawn, spawnPoint.position, Quaternion.identity);

        // �÷��̾ prawn�� ������ �θ� ��ü�Դϴ�.
        // ���� �÷��̾� ������Ʈ ������ ���� �����ؾ� �� �� �ֽ��ϴ�.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // ������ ��ü�� �÷��̾��� �ڽ����� �����մϴ�.
        instance.transform.parent = playerObject.transform;

        Managers.Instance.IsGrab = true;

    }


    private void OnCollisionEnter(Collision collision)
    {
        // Ư�� ������ �˻��Ͽ� �浹�� ����
        if (collision.gameObject.CompareTag("Food"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    player = collision.gameObject.GetComponent<Player>();
            
        //}
    }

}
