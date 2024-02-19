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
            Managers.Sound.Play( "AudioClip/Grab_On", Define.Sound.Effect);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject selectObj = _playerSM.SelectObj;
            Transform playerSpawnPos = _playerSM.SpawnPos;

            if (selectObj == null)
                return;

            if(selectObj.tag == "PlateReturn") // ���ý���
            {
                PlateReturn plateReturn = selectObj.GetComponent<PlateReturn>();
                string plateName = "Plate";

                if (plateReturn.CurrentPlateNumber > 0 && playerSpawnPos.childCount < 1)
                {
                    if (plateReturn.PlateSpawnPos.childCount == 0)
                        return;
                    Managers.Resource.Instantiate(plateName, playerSpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, playerSpawnPos);
                    Managers.Resource.Destroy(plateReturn.PlateSpawnPos.GetChild(plateReturn.PlateSpawnPos.childCount - 1).gameObject);
                    
                }

            }

            if(selectObj.tag == "Crate" && playerSpawnPos.childCount < 1) // CrateBox
            {

                CrateBoxControl Cratebox = selectObj.GetComponent<CrateBoxControl>();
                Cratebox._animator.SetTrigger("IsOpen");

                string boxName = "Crate_";
                string food_name = selectObj.transform.parent.parent.name;
                string name = food_name.Remove(0, boxName.Length);

                Vector3 newPosition = playerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y���� 1��ŭ �ø�
                Managers.Resource.Instantiate(name, newPosition, Quaternion.identity, playerSpawnPos.transform);

            }


            if (selectObj.tag == "Food") // Tag�� �̿��� �ٴڿ� �������ִ� Food �ݱ��ڵ�
            {
                string clone = "_Drop(Clone)";
                string fallingObjectName = selectObj.transform.name;
                fallingObjectName = fallingObjectName.Replace(clone, "");

                //if(FallingObjectName == "Fish")
                //{
                //    Vector3 newPosition = PlayerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y���� 0.3��ŭ �ø�
                //    Managers.Resource.Instantiate(FallingObjectName, newPosition, Quaternion.identity, PlayerSpawnPos.transform);
                //    Managers.Resource.Destroy(selectObj);
                //}
                //else
                //{
                //    Managers.Resource.Instantiate(FallingObjectName, PlayerSpawnPos.position+ new Vector3(0f, 0.3f, 0f), Quaternion.identity, PlayerSpawnPos);
                //    Managers.Resource.Destroy(selectObj);
                //}      
                Vector3 newPosition = playerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y���� 0.3��ŭ �ø�
                Managers.Resource.Instantiate(fallingObjectName, newPosition, Quaternion.identity, playerSpawnPos.transform);
                Managers.Resource.Destroy(selectObj);
            }

            if (selectObj.tag == "CuttingBoard" && _playerSM.FoodGrab == false) // �������� �ִ� ������Ʈ �ѹ��̶� ��� ����� �ϴ� �ڵ�
                return;

            if (selectObj.transform.Find("SpawnPos") == null)
                return;

            else if (selectObj.transform.Find("SpawnPos").childCount == 1)
            {

                Transform table = selectObj.transform.Find("SpawnPos");
                string clone = "(Clone)";
                string tableObjectName = selectObj.transform.Find("SpawnPos").GetChild(0).name;
                tableObjectName = tableObjectName.Replace(clone, "");

                Vector3 newPosition = playerSpawnPos.position + new Vector3(0f, 0.3f, 0f); // y���� 0.3��ŭ �ø�
                Managers.Resource.Instantiate(tableObjectName, newPosition, Quaternion.identity, playerSpawnPos);
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
