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

        // Grab 조건
        if (_playerSM.SpawnPos.childCount > 0)
        {
            Managers.Sound.Play("Effect/Game/Grab_On", Define.Sound.Effect);
            
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
        }

        // Chop 조건
        if (_playerSM.CanCut && Input.GetKey(KeyCode.LeftControl))
        {
            _stateMachine.ChangeState(_playerSM.ChopState);
        }

        // Move 조건
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            _stateMachine.ChangeState(_playerSM.MovingState);
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
