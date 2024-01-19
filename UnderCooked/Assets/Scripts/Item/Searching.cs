using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Searching : MonoBehaviour
{
    // table의 색 변화
    private Material _commonMaterial;
    private Material _instanceMaterial;

    // 이벤트 호출
    public delegate void ObjectTriggeredHandler(string name);
    public static event ObjectTriggeredHandler OnObjectTriggered;



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
            OnObjectTriggered(this.name);
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
