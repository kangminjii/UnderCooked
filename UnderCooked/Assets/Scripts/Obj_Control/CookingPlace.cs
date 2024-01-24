using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private Player player;
    private GameObject _cuttingBoard;
    private GameObject _cookingKnife;
    private GameObject _onDomaObject;
    private Transform _spawnPos;
    public int guage;
    private bool onDoma;
    private bool IsInDoma = false;


    //private GameObject[] Food;

    private void Start()
    {
        _cuttingBoard = this.transform.gameObject;
            
        _cookingKnife = _cuttingBoard.transform.Find("CuttingBoard_Knife").gameObject;//this.transform.Find("CuttingBoard_Knife").gameObject;
        _spawnPos = _cuttingBoard.transform.Find("SpawnPos");
        //player = GetComponent<Player>();
        //_playerObject = GameObject.Find("Chef");
       

    }

    private void Update()
    {
        if (onDoma && Managers.Instance.IsGrab && Input.GetKeyDown(KeyCode.Space))
        {
            // ÀÌ µÎÁÙ Fish¶û PrawnÀÌ¶û ³ª´²¾ßÇÔ
            Managers.Instance.IsGrab = false;
            _cookingKnife.SetActive(false);
            //
            if(Managers.Instance.IsPick_Prawn && !IsInDoma)
            {
                GameObject instance = Managers.Resource.Instantiate("Doma_Prawn", _spawnPos.position, Quaternion.identity, _cuttingBoard.transform);
                _onDomaObject = instance;
                Managers.Resource.Destroy(Managers.Resource.PlayerGrabItem[0]);

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
