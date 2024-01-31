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
        _playerSM.Animator.Play("Chop");
        _playerSM.Animator.SetBool("Cutting", true);
        _playerSM.Animator.SetFloat("speed", 0);

        CookingPlace.Food_Enter -= Choping;
        CookingPlace.Food_Enter += Choping;
    }

    public override void Exit()
    {
        base.Exit();
        _playerSM.Knife.SetActive(false);
        _playerSM.Animator.SetBool("Cutting", false);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_playerSM.canCut)
            _stateMachine.ChangeState(_playerSM.IdleState);
        
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


    private void Choping(GameObject ChopObject)
    {
       
    }
}

