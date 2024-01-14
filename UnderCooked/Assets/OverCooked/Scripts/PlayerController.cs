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

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        PlayerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            OnKeyboard();
        }

         
    }

    void OnKeyboard()
    {

        Vector3 moveDirection = Vector3.zero;

        //if (Dash)
        //    return;


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
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.06f);
        }





        PlayerRigidbody.position += moveDirection.normalized * Time.deltaTime * _speed;
      
       
        LookDir = transform.forward;


        if (Input.GetKey(KeyCode.LeftShift) && Time.time > LastDashTime && Dash)
        {


            float dashForce = 7f;

            PlayerRigidbody.velocity = LookDir * dashForce;

            PlayerRigidbody.AddForce(LookDir * dashForce, ForceMode.Force);

            LastDashTime = Time.time + DashCoolDown;

            Dash = false;

                
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
            ResetDash();

    }

    void ResetDash()
    {
        Dash = true;
    }

}
