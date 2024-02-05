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

        if (_playerSM.SpawnPos.childCount < 1)
            SetState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            string clone = "(Clone)";
            string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            grabObjectName = grabObjectName.Replace(clone, "");


 
            if(_playerSM.SelectObj != null && _playerSM.SelectObj.tag == "Food")
            {
                SetState();
                Managers.Resource.Instantiate(grabObjectName + "_Drop", _playerSM.SpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
                return;
            }

            if (_playerSM.SelectObj == null)
            {

                SetState();
                Managers.Resource.Instantiate(grabObjectName + "_Drop", _playerSM.SpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
                return;

            }

            if(_playerSM.SelectObj.tag == "Passing" && _playerSM.SpawnPos.GetChild(0).tag == "Plate")
            {
                SetState();
                Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
            }
            else
            {

                Transform table = _playerSM.SelectObj.transform.Find("SpawnPos");

                if (table != null && table.childCount < 1)
                {
                    if (_playerSM.SelectObj.tag == "PlateReturn" || _playerSM.SelectObj.tag == "Passing")
                        return;
                    if (grabObjectName == "Fish") // Fish �϶� Y�� ����
                    {
                        Vector3 newPosition = table.position + new Vector3(0f, 0.3f, 0f); // y���� 0.3��ŭ �ø�
                        Managers.Resource.Instantiate(grabObjectName, newPosition, Quaternion.identity, table);
                    }
                    else
                    Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);

                    Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
                    
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

    private void SetState()
    {
        _playerSM.Animator.SetBool("Grab", false);
        _stateMachine.ChangeState(_playerSM.IdleState);
    }
}
