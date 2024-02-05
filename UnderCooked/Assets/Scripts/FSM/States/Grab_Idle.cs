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
            string clone = "(Clone)";
            string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            grabObjectName = grabObjectName.Replace(clone, "");

           
            if (_playerSM.SelectObj == null)
            {
                _playerSM.Animator.SetBool("Grab", false);
                Managers.Resource.Instantiate(grabObjectName + "_Drop", _playerSM.SpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
                _stateMachine.ChangeState(_playerSM.IdleState);
            }
            else
            {
                Transform table = _playerSM.SelectObj.transform.Find("SpawnPos");
                
                if(table != null)
                {
                    if (table.childCount < 1)
                    {
                        _playerSM.Animator.SetBool("Grab", false);
                        Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);
                        Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
                        _stateMachine.ChangeState(_playerSM.IdleState);
                    }
                }
                else // spawnpos พ๘ดย passing 
                {
                    _playerSM.Animator.SetBool("Grab", false);
                    Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
                    _stateMachine.ChangeState(_playerSM.IdleState);
                }
            }
        }


        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.GrabMovingState);

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            _playerSM.Dash();
    }
}
