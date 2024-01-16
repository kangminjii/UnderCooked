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

        if (Input.anyKey == true)
            stateMachine.ChangeState(_sm.movingState);
    }

}
