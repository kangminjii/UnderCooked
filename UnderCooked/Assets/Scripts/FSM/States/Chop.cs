using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chop : Grab
{
    private float _speed = 5.0f;

    public Chop(Player stateMachine) : base("Chop", stateMachine)
    {
        _sm = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _sm.anim.SetBool("Cutting", true);
        
        
        if (!_sm.Cutting)
        {
            _sm.anim.SetBool("Cutting", false);
            stateMachine.ChangeState(_sm.idleState);
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

        _sm.anim.SetFloat("speed", _speed);

        if (Input.GetKey(KeyCode.UpArrow))
            moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.DownArrow))
            moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.RightArrow))
            moveDirection += Vector3.right;

        _sm.rigidbody.position += moveDirection.normalized * Time.deltaTime * _speed;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _sm.transform.rotation = Quaternion.Slerp(_sm.transform.rotation, toRotation, 0.06f);
        }

        _sm.lookDir = _sm.transform.forward;
    }


}

