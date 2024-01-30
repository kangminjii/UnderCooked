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
        _playerSM.Animator.SetFloat("speed", 0);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Managers.Resource.PlayerGrabItem.Count > 0)
        {
            _playerSM.Animator.SetBool("Grab", true);
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
        }

        if (_playerSM.canCut && Input.GetKey(KeyCode.LeftControl))
            _stateMachine.ChangeState(_playerSM.ChopState);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.MovingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            Dash();
    }

    public void Dash()
    {
        float dashForce = 6f;

        _playerSM.Rigidbody.velocity = _playerSM.LookDir * dashForce;
        _playerSM.Rigidbody.AddForce(_playerSM.LookDir * dashForce, ForceMode.Force);
    }


}
