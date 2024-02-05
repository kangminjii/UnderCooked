using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : BaseState
{
    protected Player _playerSM;

    public Moving(Player stateMachine) : base("Moving", stateMachine) 
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
        base.UpdateLogic();

        if (_playerSM.transform.Find("SpawnPos").childCount > 0)
        {
            _playerSM.Animator.SetBool("Grab", true);
            _stateMachine.ChangeState(_playerSM.GrabMovingState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_playerSM.SelectObj == null)
                return;

            if (_playerSM.transform.Find("SpawnPos").childCount < 1)
            {

                if (_playerSM.SelectObj.tag == "Food") // Tag�� �̿��� �ٴڿ� �������ִ� Food �ݱ��ڵ�
                {
                    string clone = "_Drop(Clone)";
                    string FallingObjectName = _playerSM.SelectObj.transform.name;
                    FallingObjectName = FallingObjectName.Replace(clone, "");

                    Managers.Resource.Instantiate(FallingObjectName, _playerSM.SpawnPos.position, Quaternion.identity, _playerSM.SpawnPos);
                    Managers.Resource.Destroy(_playerSM.SelectObj);
                    return;
                }

                if (_playerSM.SelectObj.name == "Doma_Table" && _playerSM.FoodGrab == false) // �������� �ִ� ������Ʈ �ѹ��̶� ��� ����� �ϴ� �ڵ�
                    return;


                if (_playerSM.SelectObj.transform.Find("SpawnPos") == null)
                    return;
                else if (_playerSM.SelectObj.transform.Find("SpawnPos").childCount == 1)
                {

                    Transform table = _playerSM.SelectObj.transform.Find("SpawnPos");
                    string clone = "(Clone)";
                    string TableObjectName = _playerSM.SelectObj.transform.Find("SpawnPos").GetChild(0).name;
                    TableObjectName = TableObjectName.Replace(clone, "");


                    _playerSM.Animator.SetBool("Grab", true);
                    Managers.Resource.Instantiate(TableObjectName, _playerSM.SpawnPos.position, Quaternion.identity, _playerSM.SpawnPos);
                    Managers.Resource.Destroy(table.GetChild(0).gameObject);
                    _stateMachine.ChangeState(_playerSM.GrabIdleState);
                }

            }

        }

        if (Input.anyKey == false)
            _stateMachine.ChangeState(_playerSM.IdleState);
      
        if (_playerSM.canCut && Input.GetKey(KeyCode.LeftControl))
            _stateMachine.ChangeState(_playerSM.ChopState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _playerSM.PlayerMove();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            _playerSM.Dash();
    }

}
