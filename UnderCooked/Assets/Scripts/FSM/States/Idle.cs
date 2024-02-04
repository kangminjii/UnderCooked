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

        if (_playerSM.transform.Find("SpawnPos").childCount > 0)
        {
            _playerSM.Animator.SetBool("Grab", true);
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_playerSM.transform.Find("SpawnPos").childCount < 1)
            {

                if (_playerSM.SelectObj != null && _playerSM.SelectObj.transform.Find("SpawnPos").childCount == 1)
                {
                    
                    string clone = "(Clone)";
                    string TableObjectName = _playerSM.SelectObj.transform.Find("SpawnPos").GetChild(0).name;
                    TableObjectName = TableObjectName.Replace(clone, "");

                    Transform table = _playerSM.SelectObj.transform.Find("SpawnPos");

                    _playerSM.Animator.SetBool("Grab", true);
                    Managers.Resource.Instantiate(TableObjectName, _playerSM.SpawnPos.position, Quaternion.identity, _playerSM.SpawnPos);
                    Managers.Resource.Destroy(table.GetChild(0).gameObject);
                    _stateMachine.ChangeState(_playerSM.GrabIdleState);


                }
            }
            
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
            _playerSM.Dash();
    }

}
