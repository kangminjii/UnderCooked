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
        _playerSM.Anim.SetFloat("speed", _speed);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Managers.Resource.PlayerGrabItem.Count > 0)
        {
            _playerSM.Anim.SetBool("Grab", true);
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
        }

        if (Input.anyKey == false)
            _stateMachine.ChangeState(_playerSM.IdleState);
      
        if (_playerSM.Cutting && Input.GetKey(KeyCode.LeftControl))
            _stateMachine.ChangeState(_playerSM.ChopState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        PlayerMove();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            Dash();
    }

    void PlayerMove()
    {
        Vector3 moveDirection = Vector3.zero;

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
            PlayerRotate(moveDirection);
    }

    void PlayerRotate(Vector3 moveDir)
    {
        Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
        _playerSM.transform.rotation = Quaternion.Slerp(_playerSM.transform.rotation, toRotation, 0.06f);
        _playerSM.LookDir = _playerSM.transform.forward;
    }

    public void Dash()
    {
        float dashForce = 6f;

        _playerSM.Rigidbody.velocity = _playerSM.LookDir * dashForce;
        _playerSM.Rigidbody.AddForce(_playerSM.LookDir * dashForce, ForceMode.Force);
    }



}
