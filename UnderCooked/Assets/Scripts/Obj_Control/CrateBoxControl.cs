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

    //private Player player;
    public GameObject playerObject;

    public Animator animtor;

    bool canInteract = false;

    private void Start()
    {
        animtor = GetComponent<Animator>();
        playerObject = GameObject.Find("Chef");
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
        GameObject instance = Managers.Resource.Instantiate("Prawn", new Vector3(0f, 0.365f, 0.734f), Quaternion.identity, playerObject.transform);


        //GameObject instance = Instantiate(Prawn, spawnPoint.position, Quaternion.identity);

        // 플레이어가 prawn을 부착할 부모 객체입니다.
        // 실제 플레이어 오브젝트 구조에 따라 조정해야 할 수 있습니다.
        //GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // 생성된 객체를 플레이어의 자식으로 설정합니다.
        //instance.transform.parent = playerObject.transform;


        Managers.Instance.IsGrab = true;

    }


    private void OnCollisionEnter(Collision collision)
    {
        // 특정 조건을 검사하여 충돌을 무시
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
