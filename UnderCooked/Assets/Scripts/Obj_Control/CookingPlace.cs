using UnityEngine;

public class CookingPlace : MonoBehaviour
{
    private Player player;
    private GameObject OBJ;
    private GameObject _cuttingBoard;
    private GameObject _cookingKnife;
    private PrawnScripts _onDomaObject;
    private Transform _spawnPos;

    public int guage;
    private bool IsPlayer_InDoma;
    private bool IsFood_InDoma = false;



    //private GameObject[] Food;

    private void Start()
    {
        _cuttingBoard = this.transform.gameObject;

        _cookingKnife = _cuttingBoard.transform.Find("CuttingBoard_Knife").gameObject;//this.transform.Find("CuttingBoard_Knife").gameObject;
        _spawnPos = _cuttingBoard.transform.Find("SpawnPos");
    }

    private void Update()
    {
        if (IsPlayer_InDoma /*&& Managers.Instance.IsGrab*/ && Input.GetKeyDown(KeyCode.Space))
        {
            // ¿Ã µŒ¡Ÿ Fish∂˚ Prawn¿Ã∂˚ ≥™¥≤æﬂ«‘
            //Managers.Instance.IsGrab = false;
            _cookingKnife.SetActive(false);
            //
            if (/*Managers.Instance.IsPick_Prawn &&*/ !IsFood_InDoma)
            {
                Managers.Resource.Instantiate("Doma_Prawn", _spawnPos.position, Quaternion.identity, _cuttingBoard.transform);
                //_onDomaObject = instance;
                Managers.Resource.Destroy(Managers.Resource.PlayerGrabItem[0]);

                //Managers.Instance.IsPick_Prawn = false;
                //_onDomaObject = _cuttingBoard.transform.Find("Doma_Prawn(Clone)").gameObject;
                IsFood_InDoma = true;

            }

        }

        if (IsFood_InDoma && _onDomaObject != null)
        {
            Debug.Log(_onDomaObject);
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
            IsPlayer_InDoma = true;
        }

        // ¿·Ω√ ¡÷ºÆ
        //if (other.CompareTag("Prawn"))
        //{
        //    _onDomaObject = other.transform.GetComponent<PrawnScripts>();
        //    Debug.Log(OBJ);
        //}


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
                IsPlayer_InDoma = false;
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
