using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseState
{
    protected Player _sm;
    private bool _grounded;

    public Jumping(Player stateMachine) : base("Jumping", stateMachine) 
    {
        _sm = (Player)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.meshRenderer.material.color = Color.green;

        Vector3 vel = _sm.rigidbody.velocity;
        vel.y += _sm.jumpForce;
        _sm.rigidbody.velocity = vel;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (_grounded)
            stateMachine.ChangeState(_sm.idleState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _grounded = _sm.rigidbody.velocity.y < Mathf.Epsilon && _sm.isGrounded;
    }
}
