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

            GameObject SelectObj = _playerSM.SelectObj;
            Transform PlayerSpawnPos = _playerSM.SpawnPos;

            if (SelectObj == null)
                return;

            if(SelectObj.tag == "PlateReturn") // 접시스폰
            {
                PlateReturn plateReturn = SelectObj.GetComponent<PlateReturn>();
                string _plateName = "Plate";

                if (plateReturn.CurrentPlateNumber > 0 && PlayerSpawnPos.childCount < 1)
                {
                    if (plateReturn.PlateSpawnPos.childCount == 0)
                        return;
                    Managers.Resource.Instantiate(_plateName, PlayerSpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, PlayerSpawnPos);
                    Managers.Resource.Destroy(plateReturn.PlateSpawnPos.GetChild(plateReturn.PlateSpawnPos.childCount - 1).gameObject);
                    
                }

            }

            if(SelectObj.tag == "Crate" && PlayerSpawnPos.childCount < 1) // CrateBox
            {

                CrateBoxControl Cratebox = SelectObj.GetComponent<CrateBoxControl>();
                Cratebox._animator.SetTrigger("IsOpen");

                string boxName = "Crate_";
                string food_name = SelectObj.transform.parent.parent.name;
                string name = food_name.Remove(0, boxName.Length);

                Vector3 newPosition = PlayerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y값을 1만큼 올림
                Managers.Resource.Instantiate(name, newPosition, Quaternion.identity, PlayerSpawnPos.transform);

            }


            if (SelectObj.tag == "Food") // Tag를 이용해 바닥에 떨어져있는 Food 줍기코드
            {
                string clone = "_Drop(Clone)";
                string FallingObjectName = SelectObj.transform.name;
                FallingObjectName = FallingObjectName.Replace(clone, "");

                //if(FallingObjectName == "Fish")
                //{
                //    Vector3 newPosition = PlayerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y값을 0.3만큼 올림
                //    Managers.Resource.Instantiate(FallingObjectName, newPosition, Quaternion.identity, PlayerSpawnPos.transform);
                //    Managers.Resource.Destroy(SelectObj);
                //}
                //else
                //{
                //    Managers.Resource.Instantiate(FallingObjectName, PlayerSpawnPos.position+ new Vector3(0f, 0.3f, 0f), Quaternion.identity, PlayerSpawnPos);
                //    Managers.Resource.Destroy(SelectObj);
                //}      
                Vector3 newPosition = PlayerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y값을 0.3만큼 올림
                Managers.Resource.Instantiate(FallingObjectName, newPosition, Quaternion.identity, PlayerSpawnPos.transform);
                Managers.Resource.Destroy(SelectObj);
            }

            if (SelectObj.name == "Doma_Table" && _playerSM.FoodGrab == false) // 도마위에 있는 오브젝트 한번이라도 썰면 못잡게 하는 코드
                return;

            if (SelectObj.transform.Find("SpawnPos") == null)
                return;

            else if (SelectObj.transform.Find("SpawnPos").childCount == 1)
            {

                Transform table = SelectObj.transform.Find("SpawnPos");
                string clone = "(Clone)";
                string TableObjectName = SelectObj.transform.Find("SpawnPos").GetChild(0).name;
                TableObjectName = TableObjectName.Replace(clone, "");

                Vector3 newPosition = PlayerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y값을 0.3만큼 올림
                Managers.Resource.Instantiate(TableObjectName, newPosition, Quaternion.identity, PlayerSpawnPos);
                Managers.Resource.Destroy(table.GetChild(0).gameObject);
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
