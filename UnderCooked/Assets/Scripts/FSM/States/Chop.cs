using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chop : BaseState
{
    protected Player _playerSM;
    private float _speed = 5.0f;

    public Chop(Player stateMachine) : base("Chop", stateMachine)
    {
        _playerSM = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        _playerSM.Knife.SetActive(true);
        _playerSM.Anim.Play("Chop");
        _playerSM.Anim.SetBool("Cutting", true);
    }

    public override void Exit()
    {
        base.Exit();
        _playerSM.Knife.SetActive(false);
        _playerSM.Anim.SetBool("Cutting", false);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!_playerSM.Cutting)
        {
            _stateMachine.ChangeState(_playerSM.IdleState);
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

