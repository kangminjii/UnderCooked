using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_Moving : BaseState
{
    protected Player _playerSM;
    float _speed = 5.0f;
    GameObject _ingredient;

    public Grab_Moving(Player stateMachine) : base("Grab_Moving", stateMachine)
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
        // Grab «ÿ¡¶
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSM.Anim.SetBool("Grab", false);
            _stateMachine.ChangeState(_playerSM.IdleState);

            if (_playerSM.Doma == null)
            {
                _ingredient = Managers.Resource.Instantiate("Prawn_Drop", _playerSM.SpawnPoint.position, Quaternion.identity);
                Managers.Resource.Destroy(Managers.Resource.PlayerGrabItem[0]);
                _ingredient.layer = LayerMask.NameToLayer("Default");
            }
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
