using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody PlayerRigidbody;
    private float _speed = 7.0f;

    bool Run = false;
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

        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        //if (moveDirection != Vector3.zero)
        //{

        //    //Quaternion fromRotation = transform.rotation;
        //    Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.05f);
        //    //transform.rotation = Quaternion.Slerp(fromRotation, toRotation, 0.03f);
        //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f);

        //    transform.position += moveDirection * _speed * Time.deltaTime;

        //    //transform.Translate(moveDirection * Time.deltaTime * _speed);
        //    // transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        //    //transform.position += Vector3.forward * Time.deltaTime * _speed;
        //}



        //if(Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.A))
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.06f);
        //    transform.position += (Vector3.forward * Time.deltaTime * _speed + Vector3.left * Time.deltaTime * _speed) * 0.2f; 
        //}




        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.06f);
        //    transform.position += Vector3.forward.normalized * Time.deltaTime * _speed;

        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.06f);
        //    transform.position += Vector3.left * Time.deltaTime * _speed;
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.06f);
        //    transform.position += Vector3.back * Time.deltaTime * _speed;
        //}



        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.06f);
        //    transform.position += Vector3.right * Time.deltaTime * _speed;
        //}


        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = 10f;
        }
        else _speed = 6f;
        
        
        

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.06f);
        }

       
        PlayerRigidbody.position += moveDirection.normalized * Time.deltaTime * _speed;
       

    }
}
