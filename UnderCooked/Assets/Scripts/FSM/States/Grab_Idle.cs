using UnityEngine;

public class Grab_Idle : BaseState
{
    protected Player _playerSM;



    public Grab_Idle(Player stateMachine) : base("Grab_Idle", stateMachine)
    {
        _playerSM = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        _playerSM.Animator.SetBool("Grab", true);
        _playerSM.Animator.SetFloat("speed", 0);
    }

   

    public override void UpdateLogic()
    {
        // GrabMoving 조건
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
           Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            _stateMachine.ChangeState(_playerSM.GrabMovingState);
        }

        // Idle 조건
        if (_playerSM.SpawnPos.childCount < 1)
        {
            Managers.Sound.Play("AudioClip/Grab_Off", Define.Sound.Effect);

            _playerSM.Animator.SetBool("Grab", false);
            _stateMachine.ChangeState(_playerSM.IdleState);
        }

        // Object와 상호작용시
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
