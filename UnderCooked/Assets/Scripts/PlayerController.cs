using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody PlayerRigidbody;
    private float _speed = 5.0f;
    Vector3 LookDir;



    float DashCoolDown = 0.6f;
    float LastDashTime = -Mathf.Infinity;
    bool Dash = true;
    bool Move = false;

    float wait_run_ratio = 0;

    public enum PlayerState
    {
        Idle,
        Walk,
        Dash,
        Pick,
        Coop,
        Drop
    }

    PlayerState _state = PlayerState.Idle;

    void UpdateIdle()
    {
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 5.0f * Time.deltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("wait_run_ratio", wait_run_ratio);
        anim.Play("WAIT_RUN");

        if (Input.GetKey(KeyCode.DownArrow))
            _state = PlayerState.Walk;
    }

    void UpdateWalk()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection += Vector3.left;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection += Vector3.back;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection += Vector3.right;
        }

        if (moveDirection != Vector3.zero)
        {
            //Move = true;
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.06f);
        }
        else
        {
            // Move = false;
            _state = PlayerState.Idle;
        }

        PlayerRigidbody.position += moveDirection.normalized * Time.deltaTime * _speed;

        LookDir = transform.forward;

        //애니메이션
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 5.0f * Time.deltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("wait_run_ratio", wait_run_ratio);
        anim.Play("WAIT_RUN");
    }


    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        PlayerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        OnKeyboard();

        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Walk:
                UpdateWalk();
                break;

        }

    }

    void OnKeyboard()
    {
        

        if (Input.GetKey(KeyCode.LeftShift) && Time.time > LastDashTime && Dash)
        {


            float dashForce = 7f;

            PlayerRigidbody.velocity = LookDir * dashForce;

            PlayerRigidbody.AddForce(LookDir * dashForce, ForceMode.Force);

            LastDashTime = Time.time + DashCoolDown;

            
            Dash = false;

        }
        else
        {
            ResetDash();
            
        }
        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //ResetDash();


       
    }

    void ResetDash()
    {
        Dash = true;
    }

}
