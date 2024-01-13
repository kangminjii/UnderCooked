using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grounded
{
    private float _horizontalInput;

    public Idle(MovementSM stateMachine) : base("Idle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _horizontalInput = 0f;
        ((MovementSM)stateMachine).meshRenderer.material.color = Color.black;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _horizontalInput = Input.GetAxis("Horizontal");
        // transition to "moving" state if input != 0
        if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon)
            stateMachine.ChangeState(((MovementSM)stateMachine).movingState);
    }

}
