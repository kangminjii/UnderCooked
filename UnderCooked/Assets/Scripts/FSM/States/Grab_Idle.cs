using System.Collections;
using System.Collections.Generic;
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
    }

    public override void UpdateLogic()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _playerSM.Anim.SetBool("Grab", false);

            // 바닥에 놓을 때
            if (_playerSM.Doma == null && Managers.Instance.IsGrab && Managers.Instance.IsPick_Prawn)
            {
                Managers.Instance.IsPick_Prawn = false;
                Managers.Instance.IsGrab = false;
                Managers.Instance.IsDrop = true;
                Managers.Resource.Instantiate("Prawn", Vector3.zero, Quaternion.identity);
                Managers.Resource.Destroy(Managers.Resource.PlayerGrabItem[0]);
            }

            _stateMachine.ChangeState(_playerSM.IdleState);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.GrabMovingState);

        if (Input.GetKey(KeyCode.LeftAlt))
            Dash();
    }

    public void Dash()
    {
        float dashForce = 6f;

        _playerSM.Rigidbody.velocity = _playerSM.LookDir * dashForce;
        _playerSM.Rigidbody.AddForce(_playerSM.LookDir * dashForce, ForceMode.Force);
    }
}
