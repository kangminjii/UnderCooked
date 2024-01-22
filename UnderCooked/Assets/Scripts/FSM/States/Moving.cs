using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : BaseState
{
    protected Player _playerSM;

    private float _speed = 5.0f;


    public Moving(Player stateMachine) : base("Moving", stateMachine) 
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

        if (Managers.Instance.IsGrab == true)
        {
            //_playerSM.Anim.Play("Walk_Holding");
            _playerSM.Anim.SetBool("Grab", true);
        }
        else
        {
            _playerSM.Anim.SetBool("Grab", false);
        }



        if (Input.anyKey == false)
            _stateMachine.ChangeState(_playerSM.IdleState);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {         
            _stateMachine.ChangeState(_playerSM.DashState);
        }

        if (_playerSM.Cutting && Input.GetKey(KeyCode.LeftControl))
        {
            _stateMachine.ChangeState(_playerSM.ChopState);
        }

    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        OnKeyboard();
    }

    void OnKeyboard()
    {
        Vector3 moveDirection = Vector3.zero;

        _playerSM.Anim.SetFloat("speed", _speed);
              
        if (Input.GetKey(KeyCode.UpArrow))
            moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.DownArrow))
            moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.RightArrow))
            moveDirection += Vector3.right;

        _playerSM.Rigidbody.position += moveDirection.normalized * Time.deltaTime * _speed;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _playerSM.transform.rotation = Quaternion.Slerp(_playerSM.transform.rotation, toRotation, 0.06f);
        }

        _playerSM.LookDir = _playerSM.transform.forward;



    }

}
