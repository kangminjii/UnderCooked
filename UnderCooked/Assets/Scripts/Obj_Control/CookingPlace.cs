using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private Player player;
    private GameObject CookingKnife;
    public GameObject On_Prawn;
    public int guage;
    public Transform SpawnPos;
    private bool onDoma;
    private bool IsInDoma = false;


    //private GameObject[] Food;

    private void Start()
    {
        CookingKnife = this.transform.Find("CuttingBoard_Knife").gameObject;
        player = GetComponent<Player>();
        

    }

    private void Update()
    {
        if (onDoma && Input.GetKeyDown(KeyCode.Space))
        {
            CookingKnife.SetActive(false);
            if(!IsInDoma)
            {
                Instantiate(On_Prawn, this.SpawnPos.position, Quaternion.identity);
                IsInDoma = true;
                
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("doma on");
            player = other.transform.GetComponent<Player>();

            
            player.Doma = this;
            player.Cutting = true;
            onDoma = true;


        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.CheckDoma(this.transform);
            if (player.Doma != this)
            {
                //player.Cutting = false;
                //Debug.Log("doma out");
                //player.CheckDoma(this.transform);
                player = null;
                onDoma = false;
            }
            
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Cutting = true;

        }

    }

}
