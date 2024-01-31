using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_Moving : BaseState
{
    protected Player _playerSM;
    float _speed = 5.0f;


    public Grab_Moving(Player stateMachine) : base("Grab_Moving", stateMachine)
    {
        _playerSM = (Player)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _playerSM.Animator.SetFloat("speed", _speed);
    }

    public override void UpdateLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSM.Animator.SetBool("Grab", false);
            _stateMachine.ChangeState(_playerSM.IdleState);

            Transform table = _playerSM.EnterTriggeredObject.transform.Find("SpawnPos");

            string clone = "(Clone)";
            string grabObjectName = _playerSM.SpawnPos.GetChild(0).name;
            grabObjectName = grabObjectName.Replace(clone, "");

            if (_playerSM.EnterTriggeredObject == _playerSM.ExitTriggeredObject)
            {
                Managers.Resource.Instantiate(grabObjectName + "_Drop", _playerSM.SpawnPos.position, Quaternion.identity);
            }
            else
            {
                Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);
            }

            Managers.Resource.Destroy(_playerSM.SpawnPos.GetChild(0).gameObject);
        }

        if (Input.anyKey == false)
            _stateMachine.ChangeState(_playerSM.GrabIdleState);
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
