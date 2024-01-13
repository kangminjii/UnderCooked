using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private float _speed = 7.0f;
    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
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

        //    Quaternion fromRotation = transform.rotation;
        //    Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        //    transform.rotation = Quaternion.Slerp(fromRotation, toRotation, 0.03f);


        //    //transform.Translate(moveDirection * Time.deltaTime * _speed);
        //    transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        //    //transform.position += Vector3.forward * Time.deltaTime * _speed;
        //}

        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.06f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;

        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.06f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.06f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.06f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }

        

    }
}
