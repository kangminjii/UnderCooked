using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    protected Player _playerSM;
    
    float _initTime;
    
    public Dash(Player stateMachine) : base("Dash", stateMachine)
    {
        _playerSM = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();
        _initTime = 0;
        //_sm.SetCooldown();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        
        _stateMachine.ChangeState(_playerSM.MovingState);
            
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        OnAlt();
    }

    public void OnAlt()
    {

        float dashForce = 6f;

        _playerSM.Rigidbody.velocity = _playerSM.LookDir * dashForce;

        _playerSM.Rigidbody.AddForce(_playerSM.LookDir * dashForce, ForceMode.Force);

        //_playerSM.LastDashTime = Time.time + _playerSM.DashCoolDown;

        //if (Input.GetKeyUp(KeyCode.LeftShift))


    }

}
