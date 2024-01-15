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
    public Jumping jumpingState;

    public Rigidbody rigidbody;
    public MeshRenderer meshRenderer;

    public float speed = 4f;
    public float jumpForce = 10f;


    private void Awake()
    {
        idleState = new Idle(this);
        movingState = new Moving(this);
        jumpingState = new Jumping(this);
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

}
