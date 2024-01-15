using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ¾È¾¸


public class Grounded : BaseState
{
    protected Player _sm;

    public Grounded(string name, Player stateMachine) : base(name, stateMachine) 
    {
        _sm = (Player)stateMachine;
    }

    //public override void UpdateLogic()
    //{
    //    base.UpdateLogic();
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        stateMachine.ChangeState(_sm.jumpingState);
    //}
}
