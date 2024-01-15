using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Grounded
{
    private float _horizontalInput;

    public Moving(Player stateMachine) : base("Moving", stateMachine) 
    {
        _sm = (Player)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _horizontalInput = 0f;
        _sm.meshRenderer.material.color = Color.red;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(_horizontalInput) < Mathf.Epsilon)
            stateMachine.ChangeState(_sm.idleState);
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Vector3 vel = _sm.rigidbody.velocity;
        vel.x = _horizontalInput * _sm.speed;
        _sm.rigidbody.velocity = vel;
    }
}
