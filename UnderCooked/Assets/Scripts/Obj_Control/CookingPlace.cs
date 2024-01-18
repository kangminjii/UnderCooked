using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private Player player;
    public int guage;

    private GameObject food;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("doma on");
            player = other.transform.GetComponent<Player>();

            player.doma = this;
            player.Cutting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("doma out");           
            player.CheckDoma(this.transform);

            //player.Cutting = false;

            player = null;

        }
    }

    public void Cooking()
    {
        
    }
}
