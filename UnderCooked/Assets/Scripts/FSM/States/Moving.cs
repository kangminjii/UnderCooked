using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : BaseState
{
    protected Player _playerSM;

    public Moving(Player stateMachine) : base("Moving", stateMachine) 
    {
        _playerSM = (Player)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _playerSM.Animator.SetFloat("speed", _playerSM._speed);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_playerSM.transform.Find("SpawnPos").childCount > 0)
        {
            _playerSM.Animator.SetBool("Grab", true);
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
        }

        if (Input.anyKey == false)
            _stateMachine.ChangeState(_playerSM.IdleState);
      
        if (_playerSM.canCut && Input.GetKey(KeyCode.LeftControl))
            _stateMachine.ChangeState(_playerSM.ChopState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _playerSM.PlayerMove();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            _playerSM.Dash();
    }

}
