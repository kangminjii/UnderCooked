using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Searching : MonoBehaviour
{
    // table의 색 변화
    private Material _commonMaterial;
    private Material _instanceMaterial;

    private GameObject Table;
    private Transform Table_Spawn;


    // 이벤트 호출
    public delegate void ObjectTriggeredHandler(GameObject gameObject);
    public static event ObjectTriggeredHandler ObjectTriggerEnter;
    public static event ObjectTriggeredHandler ObjectTriggerExit;



    private void Start()
    {
        _commonMaterial = GetComponent<MeshRenderer>().material;
        _instanceMaterial = Instantiate(_commonMaterial);
        GetComponent<MeshRenderer>().material = _instanceMaterial;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ObjectTriggerEnter(this.gameObject);
        }

        if (other.tag == "Food")
        {
            GameObject prawnObject = other.gameObject;
            Destroy(prawnObject);


            Table = this.gameObject;
            Table_Spawn = this.transform.Find("SpawnPos");
            Managers.Resource.Instantiate("Prawn", Table_Spawn.position, Quaternion.identity, Table_Spawn);
            //Managers.Instance.IsGrab = false;
            //Managers.Instance.IsPick_Prawn = false;
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjectTriggerExit(this.gameObject);
        }
    }


    public void EnableColor()
    {
        Color newEmissionColor = new Color(0.5f, 0.45f, 0.4f, 0f);
        _instanceMaterial.SetColor("_EmissionColor", newEmissionColor);
        _instanceMaterial.EnableKeyword("_EMISSION");
    }

    public void DisableColor()
    {
        _instanceMaterial.DisableKeyword("_EMISSION");
    }

}
