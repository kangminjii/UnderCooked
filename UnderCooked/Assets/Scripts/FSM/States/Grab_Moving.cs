using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_Moving : BaseState
{
    protected Player _playerSM;
    
    public static Action<string> FoodOrderCheck;


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
        if (_playerSM.SpawnPos.childCount < 1)
            SetState();

        if (Input.GetKeyDown(KeyCode.Space))
        {

            string clone = "(Clone)";
            string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            grabObjectName = grabObjectName.Replace(clone, "");

            GameObject selectObj = _playerSM.SelectObj;
            Transform playerSpawnPos = _playerSM.SpawnPos;

            if (selectObj != null && selectObj.tag == "Bin")
            {
                Transform trash = selectObj.transform.Find("BinSpawnPos");       

                Managers.Resource.Instantiate(grabObjectName, trash.position, Quaternion.identity, trash);
                Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);
            }

            if (selectObj != null && selectObj.tag == "CuttingBoard" && playerSpawnPos.GetChild(0).tag.Contains("Plate")) //도마 접시위에 올라가지않게 막음
                return;

            if (selectObj != null && selectObj.tag == "Food")
            {
                SetState();
                Managers.Resource.Instantiate(grabObjectName + "_Drop", playerSpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);
                return;
            }

            if (selectObj != null && selectObj.tag == "CuttingBoard" && playerSpawnPos.GetChild(0).tag.Contains("Plate"))
                return;

            if (_playerSM.SelectObj == null)
            {
                SetState();
                Managers.Resource.Instantiate(grabObjectName + "_Drop", playerSpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);
                return;
            }

            if (selectObj.tag == "Passing") // 접시 반납
            {

                if (playerSpawnPos.GetChild(0).tag == "Plate" || playerSpawnPos.GetChild(0).tag == "EmptyPlate")
                {
                    
                    PassingGate PassingGate = selectObj.GetComponent<PassingGate>();
                    PassingGate.plateReturn.PlateList.RemoveAt(PassingGate.plateReturn.PlateList.Count - 1);
                    PassingGate.plateReturn.CurrentPlateNumber--;

                    string returnFoodName;

                    if (playerSpawnPos.GetChild(0).name.Contains("Prawn"))
                        returnFoodName = "Prawn";
                    else if (playerSpawnPos.GetChild(0).name.Contains("Fish"))
                        returnFoodName = "Fish";
                    else
                        returnFoodName = null;

                    FoodOrderCheck.Invoke(returnFoodName);

                    Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);
                }

            }
            else
            {

                Transform table = selectObj.transform.Find("SpawnPos");

                if (table == null)
                    return;

                if (selectObj.tag == "CuttingBoard" && table.childCount < 1 && playerSpawnPos.GetChild(0).tag.Contains("Plate")) //도마 위에 접시 안올라가게 함
                    return;


                if (table.childCount == 1)
                {
                    if (table.GetChild(0).tag == "EmptyPlate" && playerSpawnPos.GetChild(0).tag == "SlicedFood")
                    {
                        //Vector3 newPosition = table.position + new Vector3(0f, 0.3f, 0f); // y값을 0.3만큼 올림
                        Managers.Resource.Instantiate(grabObjectName + "_Plate", table.position, Quaternion.identity, table);
                        Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);
                        Managers.Resource.Destroy(table.GetChild(0).gameObject);
                    }
                    else if (playerSpawnPos.GetChild(0).tag == "EmptyPlate" && table.GetChild(0).tag == "SlicedFood")
                    {

                        string tableclone = "(Clone)";
                        string tableObjectName = table.GetChild(0).name;
                        tableObjectName = tableObjectName.Replace(tableclone, "");

                        Managers.Resource.Instantiate(tableObjectName + "_Plate", playerSpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, playerSpawnPos);
                        Managers.Resource.Destroy(table.GetChild(0).gameObject);
                        Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);
                    }
                }


                if (table != null && table.childCount < 1)
                {
                    if (grabObjectName == "Fish") // Fish 일때 Y값 증가
                    {
                        Vector3 newPosition = table.position + new Vector3(0f, 0.3f, 0f); // y값을 0.3만큼 올림
                        Managers.Resource.Instantiate(grabObjectName, newPosition, Quaternion.identity, table);
                    }
                    else
                        Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);

                    Managers.Resource.Destroy(playerSpawnPos.GetChild(0).gameObject);

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
