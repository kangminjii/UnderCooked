using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    protected Player _playerSM;


    public Idle(Player stateMachine) : base("Idle", stateMachine) 
    {
        _playerSM = (Player)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _playerSM.Anim.SetFloat("speed", 0);

        if (_playerSM.Cutting && Input.GetKey(KeyCode.LeftControl))
            _stateMachine.ChangeState(_playerSM.ChopState);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.MovingState);

        if (Input.GetKey(KeyCode.LeftShift))
            _stateMachine.ChangeState(_playerSM.DashState);


    }


}
