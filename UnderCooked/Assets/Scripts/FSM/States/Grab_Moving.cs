using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_Moving : BaseState
{
    protected Player _playerSM;


    public Grab_Moving(Player stateMachine) : base("Grab_Moving", stateMachine)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSM.Animator.SetBool("Grab", false);
            _stateMachine.ChangeState(_playerSM.IdleState);

            Transform table = _playerSM.EnterTriggeredObject.transform.Find("SpawnPos");

            string clone = "(Clone)";
            string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            grabObjectName = grabObjectName.Replace(clone, "");

            if (_playerSM.EnterTriggeredObject == _playerSM.ExitTriggeredObject)
            {
                Managers.Resource.Instantiate(grabObjectName + "_Drop", _playerSM.SpawnPos.position, Quaternion.identity);
            }
            else
            {
                Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);
            }

            Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
        }

        if (Input.anyKey == false)
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _playerSM.PlayerMove();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            _playerSM.Dash();
    }
}
