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
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _playerSM.Anim.SetFloat("speed", 0);



        if (Input.GetKeyDown(KeyCode.Space))
        {

            if(_playerSM.Doma == null && Managers.Instance.IsGrab && Managers.Instance.IsPick_Prawn)
            {
                Managers.Instance.IsPick_Prawn = false;
                Managers.Instance.IsGrab = false;
                Managers.Instance.IsDrop = true;
                //Managers.Instance.SpawnPlayerPrawn();
                Managers.Resource.Instantiate("Prawn", Vector3.zero, Quaternion.identity);
                Managers.Resource.Destroy(Managers.Resource.PlayerPrawn[0]);

            }

        }

        if(Managers.Instance.IsGrab == false)
        {
            _stateMachine.ChangeState(_playerSM.IdleState);
        }

        if (Managers.Instance.IsGrab == true)
        {
            //_playerSM.Anim.Play("Idle_Holding");
            _playerSM.Anim.SetBool("Grab", true);
        }
        else
        {
            _playerSM.Anim.SetBool("Grab", false);
        }

        if (_playerSM.Cutting && Input.GetKey(KeyCode.LeftControl))
            _stateMachine.ChangeState(_playerSM.ChopState);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.MovingState);

        if (Input.GetKey(KeyCode.LeftAlt))
            _stateMachine.ChangeState(_playerSM.DashState);


    }


}
