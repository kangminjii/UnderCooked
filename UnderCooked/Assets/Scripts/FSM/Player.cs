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


    public Rigidbody rigidbody;
    public Vector3 lookDir;


    private void Awake()
    {
        idleState = new Idle(this);
        movingState = new Moving(this);
        dashState = new Dash(this);

        rigidbody = GetComponent<Rigidbody>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

}
