using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    public string animName = "CrateBox";
    public KeyCode triggerKey = KeyCode.LeftControl;

    public Animator animtor;

    bool canInteract = false;

    private void Start()
    {
        animtor = GetComponent<Animator>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("tt");
            canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("tt");
            canInteract = false;
        }
    }

    private void PlayAnimation()
    {
        animtor.SetTrigger("IsOpen");
    }

    private void Update()
    {
        if(canInteract && Input.GetKeyDown(triggerKey))
        {
            PlayAnimation();
            canInteract = false;
        }
    }
}
