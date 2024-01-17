using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    protected Player _sm;
    float initTime;
    
    public Dash(Player stateMachine) : base("Dash", stateMachine)
    {
        _sm = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();
        initTime = 0;
        //_sm.SetCooldown();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        
        stateMachine.ChangeState(_sm.movingState);
            
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        OnShift();
    }

    public void OnShift()
    {

        float dashForce = 6f;

        _sm.rigidbody.velocity = _sm.lookDir * dashForce;

        _sm.rigidbody.AddForce(_sm.lookDir * dashForce, ForceMode.Force);

        _sm.lastDashTime = Time.time + _sm.dashCoolDown;

        //if (Input.GetKeyUp(KeyCode.LeftShift))


    }

}
