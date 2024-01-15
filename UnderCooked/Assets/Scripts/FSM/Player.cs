using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Moving movingState;
   

    public Rigidbody rigidbody;


    private void Awake()
    {
        idleState = new Idle(this);
        movingState = new Moving(this);

        rigidbody = GetComponent<Rigidbody>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

}
