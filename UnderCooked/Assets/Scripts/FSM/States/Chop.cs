using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chop : BaseState
{
    protected Player _playerSM;


    public Chop(Player stateMachine) : base("Chop", stateMachine)
    {
        _playerSM = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        _playerSM.Knife.SetActive(true);
        _playerSM.Animator.Play("Chop");
        _playerSM.Animator.SetBool("Cutting", true);
        _playerSM.Animator.SetFloat("speed", 0);
    }


    public override void Exit()
    {
        base.Exit();

        _playerSM.Knife.SetActive(false);
        _playerSM.Animator.SetBool("Cutting", false);
    }


    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Idle Á¶°Ç
        if (!_playerSM.CanCut)
            _stateMachine.ChangeState(_playerSM.IdleState);
    }


    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _playerSM.PlayerMove();
    }
}

