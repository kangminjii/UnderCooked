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
        if(_playerSM.SpawnPos.childCount < 1)
            SetState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            string clone = "(Clone)";
            string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            grabObjectName = grabObjectName.Replace(clone, "");

            GameObject SelectObj = _playerSM.SelectObj;
            Transform PlayerSpawnPos = _playerSM.SpawnPos;

            if (SelectObj != null && SelectObj.tag == "Food")
            {
                SetState();
                Managers.Resource.Instantiate(grabObjectName + "_Drop", PlayerSpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(PlayerSpawnPos.GetChild(0).gameObject);
                return;
            }

            if (_playerSM.SelectObj == null)
            {

                SetState();
                Managers.Resource.Instantiate(grabObjectName + "_Drop", PlayerSpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(PlayerSpawnPos.GetChild(0).gameObject);
                return;

            }

            if (SelectObj.tag == "Passing" && PlayerSpawnPos.GetChild(0).tag == "Plate") // ���� �ݳ�
            {
                SetState();
                PassingGate PassingGate = SelectObj.GetComponent<PassingGate>();
                PassingGate.plateReturn.PlateList.RemoveAt(PassingGate.plateReturn.CurrentPlateNumber - 1);
                PassingGate.plateReturn.CurrentPlateNumber--;

                Managers.Resource.Destroy(PlayerSpawnPos.GetChild(0).gameObject);
            }
            else
            {

                Transform table = SelectObj.transform.Find("SpawnPos");

                if (table != null && table.childCount < 1)
                {
                    //if (SelectObj.tag == "PlateReturn" || SelectObj.tag == "Passing")
                    //    return;
                    if (grabObjectName == "Fish") // Fish �϶� Y�� ����
                    {
                        Vector3 newPosition = table.position + new Vector3(0f, 0.3f, 0f); // y���� 0.3��ŭ �ø�
                        Managers.Resource.Instantiate(grabObjectName, newPosition, Quaternion.identity, table);
                    }
                    else
                        Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);

                    Managers.Resource.Destroy(PlayerSpawnPos.GetChild(0).gameObject);

                }
            }
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

    private void SetState()
    {
        _playerSM.Animator.SetBool("Grab", false);
        _stateMachine.ChangeState(_playerSM.MovingState);
    }
}
