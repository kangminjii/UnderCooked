using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Moving movingState;
    [HideInInspector]
    public Dash dashState;
    [HideInInspector]
    public Chop chopState;

    public Animator anim;

    public Rigidbody rigidbody;
    public Vector3 lookDir;
    public float dashCoolDown = 0.6f;
    public float lastDashTime = -Mathf.Infinity;

    public GameObject knife;

    public bool Cutting = false;


    public CookingPlace doma;

    private void Awake()
    {
        idleState = new Idle(this);
        movingState = new Moving(this);
        dashState = new Dash(this);
        chopState = new Chop(this);

        //AddState(idleState, StateName.Idle);
        //AddState(movingState, StateName.Walk);
        //AddState(dashState, StateName.Dash);

        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public void CheckDoma(Transform target)
    {
        
        if (doma != target)
        {

            Cutting = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("CuttingBoard"))
    //    {
    //        Debug.Log("CuttingBoard1111111");
    //        Cutting = true;
    //    }
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("CuttingBoard"))
    //    {
    //        Debug.Log("CuttingBoard2222222");
    //        Cutting = false;

    //    }
    //}
}
