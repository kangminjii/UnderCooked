using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    public string animName = "CrateBox";
    public KeyCode triggerKey = KeyCode.LeftControl;
    public GameObject prawn;
    public Transform spawnPoint;
    public GameObject light;

    private Player player;

    public Animator animtor;

    bool canInteract = false;
    bool spawnCheck = false;
    bool Isonfood = false;
    private void Start()
    {
        animtor = GetComponent<Animator>();
        //GameObject instance = Instantiate(prawn, spawnPoint.position, Quaternion.identity);

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

            spawnCheck = true;
            PlayAnimation();
            canInteract = false;
        }

        if (spawnCheck)
        {
            SpawnObj();
            spawnCheck = false;
        }
    }

    public void SpawnObj()
    {
        
        GameObject Instance = Instantiate(prawn);

        //spawnPoint.position = player.leftHand.transform.position;
        Instance.transform.parent = player.leftHand.transform;

        //Debug.Log(spawnPoint.position);

        //GameObject instance = Instantiate(prawn, spawnPoint.position, Quaternion.identity)

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
            player = collision.gameObject.GetComponent<Player>();
            
        }
    }

}
