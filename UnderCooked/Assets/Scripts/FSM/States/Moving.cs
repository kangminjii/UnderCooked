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

        _playerSM.Animator.SetFloat("speed", _playerSM.Speed);
    }


    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Grab 조건
        if (_playerSM.transform.Find("SpawnPos").childCount > 0)
        {
            Managers.Sound.Play("AudioClip/Grab_On", Define.Sound.Effect);

            _stateMachine.ChangeState(_playerSM.GrabMovingState);
        }

        // Chop 조건
        if (_playerSM.CanCut && Input.GetKey(KeyCode.LeftControl))
        {
            _stateMachine.ChangeState(_playerSM.ChopState);
        }

        // Idle 조건
        if (Input.anyKey == false)
        {
            _stateMachine.ChangeState(_playerSM.IdleState);
        }

        // Object와 상호작용시
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSM.InteractObject();
        }
    }


    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _playerSM.PlayerMove();
    }
}
