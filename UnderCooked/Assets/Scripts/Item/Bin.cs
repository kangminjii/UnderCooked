using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    //bool canInteract = false;
    //Transform _playerSpawnPos;
    Transform _binSpawnPos;
    AnimationClip animationClip;
    GameObject trash;

    //public PlateReturn plateReturn;
    private void Start()
    {
        _binSpawnPos = this.transform;//transform.Find("BinSpawnPos");
        //animation = Resources.Load<Animation>
        animationClip = Resources.Load<AnimationClip>("AnimationClip/BinGo");
    }

    private void Update()
    {

        if(_binSpawnPos.childCount > 0)
        {
            if (_binSpawnPos.childCount > 1)
                Destroy(trash);

            trash = _binSpawnPos.GetChild(0).gameObject;


            trash.GetComponent<Animator>().SetTrigger("binTrigger");       
        }

    }
}
