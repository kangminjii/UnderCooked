using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private Player player;
    public int guage;
    
    public bool onDoma = false;

    //private GameObject[] Food;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("doma on");
            player = other.transform.GetComponent<Player>();

            
            player.doma = this;
            player.Cutting = true;
            onDoma = true;


        }

           


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            player.CheckDoma(this.transform);
            //player.Cutting = false;
            //Debug.Log("doma out");
            //player.CheckDoma(this.transform);
            onDoma = false;
            player = null;
        }
        //else
        //{
        //    player.Cutting = false;
        //    onDoma = false;
        //}

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onDoma = true;
            player.Cutting = true;

        }
            
                
    }



    public void Cooking()
    {
        
    }
}
