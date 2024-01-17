using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grab
{
    public Idle(Player stateMachine) : base("Idle", stateMachine) 
    {
        _sm = (Player)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.anim.SetFloat("speed", 0);

        if (Input.anyKey == true)
            stateMachine.ChangeState(_sm.movingState);
        if (Input.GetKey(KeyCode.LeftShift))
            stateMachine.ChangeState(_sm.dashState);
    }


}
