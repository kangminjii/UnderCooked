using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    //bool canInteract = false;
    //Transform _playerSpawnPos;
    Transform _binSpawnPos;
    AnimationClip animationClip;
    bool flag = false;

    //public PlateReturn plateReturn;
    private void Start()
    {
        _binSpawnPos = transform.Find("BinSpawnPos");
        //animation = Resources.Load<Animation>
        animationClip = Resources.Load<AnimationClip>("AnimationClip/BinGo");
    }

    private void Update()
    {

        if(_binSpawnPos.childCount > 0)
        {
            GameObject trash = _binSpawnPos.GetChild(0).gameObject;

            trash.GetComponent<Animator>().SetTrigger("binTrigger");
           
            //Destroy(trash);
           
        }

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canInteract = true;
    //        _playerSpawnPos = other.transform.Find("SpawnPos");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canInteract = false;
    //    }
    //}
}
