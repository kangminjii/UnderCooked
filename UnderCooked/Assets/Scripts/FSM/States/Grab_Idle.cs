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
            //string clone = "(clone)";
            //string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            //grabObjectName = grabObjectName.Substring(0, grabObjectName.LastIndexOf(clone));

            if (_playerSM.EnterTriggeredObject == _playerSM.ExitTriggeredObject) // ¶¥¹Ù´Ú
            {
                Managers.Resource.Instantiate("Prawn_Drop"/*grabObjectName + "_Drop"*/, _playerSM.SpawnPos.position, Quaternion.identity);
            }
            else // Å×ÀÌºí
            {
                Managers.Resource.Instantiate("Prawn"/*grabObjectName*/, table.position, Quaternion.identity, table);
            }

            Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
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
