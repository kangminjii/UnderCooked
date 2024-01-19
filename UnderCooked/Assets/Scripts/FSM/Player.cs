using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [HideInInspector]
    public Idle IdleState;
    [HideInInspector]
    public Moving MovingState;
    [HideInInspector]
    public Dash DashState;
    [HideInInspector]
    public Chop ChopState;

    public CookingPlace Doma;

    public Animator Anim;
    public Rigidbody Rigidbody;
    public GameObject Knife;

    public Vector3 LookDir;
    public float DashCoolDown = 0.6f;
    public float LastDashTime = -Mathf.Infinity;

    public bool Cutting = false;

    private string _lastName;


    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        DashState = new Dash(this);
        ChopState = new Chop(this);

        //AddState(idleState, StateName.Idle);
        //AddState(movingState, StateName.Walk);
        //AddState(dashState, StateName.Dash);

        Rigidbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();


        Searching.OnObjectTriggered += HandleObjectTriggered;
    }

    protected override BaseState GetInitialState()
    {
        return IdleState;
    }

    public void CheckDoma(Transform target)
    {

        if (Doma != target)
        {
            Cutting = false;
        }
    }

    private void HandleObjectTriggered(string name)
    {
        if(name != _lastName)
        {
            // object 색변화
            // name 사물은 색 켜기, _lastName은 색 끄기
            Searching interactingObject = GameObject.Find(name).GetComponent<Searching>();
            interactingObject.EnableColor();

            if(_lastName != null) // ""로 해야 될때있고, null로 해야 될때가있음
            {
                Searching pastObject = GameObject.Find(_lastName).GetComponent<Searching>();
                pastObject.DisableColor();
            }

            _lastName = name;
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
