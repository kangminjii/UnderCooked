using System;
using UnityEngine;

public class Grab_Moving : BaseState
{
    protected Player _playerSM;
    
    public static Action<string> FoodOrderCheck;


    public Grab_Moving(Player stateMachine) : base("Grab_Moving", stateMachine)
    {
        _playerSM = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();
        
        _playerSM.Animator.SetBool("Grab", true);
        _playerSM.Animator.SetFloat("speed", _playerSM.Speed);
    }


    public override void UpdateLogic()
    {
        // GrabIdle ����
        if (Input.anyKey == false)
        {
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
        }

        // Idle ����
        if (_playerSM.SpawnPos.childCount == 0)
        {
            Managers.Sound.Play("AudioClip/Grab_Off", Define.Sound.Effect);
            
            _playerSM.Animator.SetBool("Grab", false);
            _stateMachine.ChangeState(_playerSM.MovingState);
        }

        // Object�� ��ȣ�ۿ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSM.InteractObjectWhileGrabbing();
        }
    }



    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _playerSM.PlayerMove();
    }
}
