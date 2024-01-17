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

        if (_sm.Cutting && Input.GetKey(KeyCode.LeftControl))
            stateMachine.ChangeState(_sm.chopState);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            stateMachine.ChangeState(_sm.movingState);

        if (Input.GetKey(KeyCode.LeftShift))
            stateMachine.ChangeState(_sm.dashState);


    }


}
