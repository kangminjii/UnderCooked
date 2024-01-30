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
        _playerSM.Animator.SetFloat("speed", 0);
    }

    public override void UpdateLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSM.Animator.SetBool("Grab", false);
            _stateMachine.ChangeState(_playerSM.IdleState);

            Transform table = _playerSM.EnterTriggeredObject.transform.Find("SpawnPos");

            if (_playerSM.EnterTriggeredObject == _playerSM.ExitTriggeredObject)
            {
                Managers.Resource.Instantiate("Prawn_Drop", _playerSM.SpawnPoint.position, Quaternion.identity);
            }
            else
            {
                Managers.Resource.Instantiate("Prawn", table.position, Quaternion.identity, table);
            }

            Managers.Resource.Destroy(Managers.Resource.PlayerGrabItem[0]);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.GrabMovingState);

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
