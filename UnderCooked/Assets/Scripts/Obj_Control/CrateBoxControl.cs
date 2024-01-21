using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    public string animName = "CrateBox";
    public KeyCode triggerKey = KeyCode.Space;
    public GameObject prawn;
    public Transform spawnPoint;
    public GameObject light;

    public Animator animtor;

    bool canInteract = false;
   
    
    

    private void Start()
    {
        animtor = GetComponent<Animator>();

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

            //spawnCheck = true;
            PlayAnimation();
            canInteract = false;
            if (Managers.Instance.IsGrab == false)
            {
                SpawnObj();
                Managers.Instance.IsGrab = true;
            }
        }



    }

    public void SpawnObj()
    {

        GameObject instance = Instantiate(prawn,spawnPoint.position, Quaternion.identity);
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");

        instance.transform.parent = playerobj.transform;
        Managers.Instance.IsGrab = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        // 특정 조건을 검사하여 충돌을 무시
        if (collision.gameObject.CompareTag("Food"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            //player = collision.gameObject.GetComponent<Player>();

        }
    }

}
