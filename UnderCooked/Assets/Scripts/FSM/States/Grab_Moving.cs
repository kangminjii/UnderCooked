using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_Moving : BaseState
{
    protected Player _playerSM;
    private float _speed = 5.0f;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 바닥에 놓을 때
            if (_playerSM.Doma == null && Managers.Instance.IsGrab && Managers.Instance.IsPick_Prawn)
            {
                _playerSM.Anim.SetBool("Grab", false);
                Managers.Instance.IsPick_Prawn = false;
                Managers.Instance.IsGrab = false;
                Managers.Resource.Instantiate("Drop_Prawn", _playerSM._playerSpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(Managers.Resource.PlayerGrabItem[0]);
                Managers.Instance.CanPickBool();
            }
            if (!Managers.Instance.IsGrab)
                _playerSM.Anim.SetBool("Grab", false);

            _stateMachine.ChangeState(_playerSM.IdleState);
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) &&
            !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            _stateMachine.ChangeState(_playerSM.GrabIdleState);

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            Dash();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        OnKeyboard();
    }

    void OnKeyboard()
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
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _playerSM.transform.rotation = Quaternion.Slerp(_playerSM.transform.rotation, toRotation, 0.06f);
        }

        _playerSM.LookDir = _playerSM.transform.forward;
    }

    public void Dash()
    {
        float dashForce = 6f;

        _playerSM.Rigidbody.velocity = _playerSM.LookDir * dashForce;
        _playerSM.Rigidbody.AddForce(_playerSM.LookDir * dashForce, ForceMode.Force);
    }

}
