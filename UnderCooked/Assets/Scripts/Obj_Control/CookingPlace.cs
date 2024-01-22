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
        

    }

    private void Update()
    {
        if (onDoma == true && Managers.Instance.IsGrab == true && Input.GetKeyDown(KeyCode.Space))
        {
            Managers.Instance.IsGrab = false;
            CookingKnife.SetActive(false);
            if(Managers.Instance.IsPick_Prawn == true && !IsInDoma)
            {
                Instantiate(On_Prawn, this.SpawnPos.position, Quaternion.identity);
                Managers.Instance.IsPick_Prawn = false;
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
            if (player.Doma != this.transform)
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
