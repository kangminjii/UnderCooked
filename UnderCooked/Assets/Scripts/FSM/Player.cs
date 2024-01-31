using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    // ����
    [HideInInspector]
    public Idle IdleState;
    [HideInInspector]
    public Moving MovingState;
    [HideInInspector]
    public Chop ChopState;
    [HideInInspector]
    public Grab_Idle GrabIdleState;
    [HideInInspector]
    public Grab_Moving GrabMovingState;

    // :: Public
    public Animator Animator;
    public Rigidbody Rigidbody;
    public Transform SpawnPos;
    public GameObject Knife;

    public Vector3 LookDir;
    public bool canCut;
    public float _speed = 5.0f;

    public GameObject EnterTriggeredObject = null; // �������� ��ü
    public GameObject ExitTriggeredObject = null;   // �������� ��ü
    
    
    // :: Private


    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        ChopState = new Chop(this);
        GrabIdleState = new Grab_Idle(this);
        GrabMovingState = new Grab_Moving(this);

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        SpawnPos = this.transform.Find("SpawnPos");

        Searching.ObjectTriggerEnter -= HandleObjectTriggerEnter;
        Searching.ObjectTriggerExit -= HandleObjectTriggerExit;
        Searching.ObjectTriggerEnter += HandleObjectTriggerEnter;
        Searching.ObjectTriggerExit += HandleObjectTriggerExit;
    }

    protected override BaseState GetInitialState()
    {
        return IdleState;
    }


    // Player ������
    public void PlayerMove()
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

        Rigidbody.position += moveDirection.normalized * Time.deltaTime * _speed;

        if (moveDirection != Vector3.zero)
            PlayerRotate(moveDirection);
    }

    public void PlayerRotate(Vector3 moveDir)
    {
        Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.06f);
        LookDir = transform.forward;
    }

    public void Dash()
    {
        float dashForce = 6f;

        Rigidbody.velocity = LookDir * dashForce;
        Rigidbody.AddForce(LookDir * dashForce, ForceMode.Force);
    }


    private void HandleObjectTriggerEnter(GameObject triggeredObject)
    {
        TriggeredObject(triggeredObject);

        // �ٸ� Table�� Trigger�ɶ�
        if (EnterTriggeredObject != null)
        {
            if(triggeredObject.name != EnterTriggeredObject.name)
            {
                Searching interactingObject = triggeredObject.GetComponent<Searching>();
                interactingObject.EnableColor();

                // TriggerExit���� �Ȳ��� Object ó��
                Searching pastObject = EnterTriggeredObject.GetComponent<Searching>();
                pastObject.DisableColor();
            }
        }
        // ���� Table�� Trigger�ɶ�
        else
        {
            if (ExitTriggeredObject == triggeredObject)
            {
                Searching interactingObject = triggeredObject.GetComponent<Searching>();
                interactingObject.EnableColor();
            }
        }

        EnterTriggeredObject = triggeredObject;
    }

    private void HandleObjectTriggerExit(GameObject triggeredObject)
    {
        TriggeredObjectExit(triggeredObject);

        Searching interactingObject = triggeredObject.GetComponent<Searching>();
        interactingObject.DisableColor();

        ExitTriggeredObject = triggeredObject;
    }

    private void TriggeredObjectExit(GameObject triggeredObject)
    {
        Define.Object objectType = GetObjectFromTag(triggeredObject.tag);

        switch (objectType)
        {
            case Define.Object.CuttingBoard:
                canCut = false;
                break;
            case Define.Object.Default:
                break;
        }
    }

    private void TriggeredObject(GameObject triggeredObject)
    {
        Define.Object objectType = GetObjectFromTag(triggeredObject.tag);

        switch (objectType)
        {
            case Define.Object.CuttingBoard:
                canCut = true;
                break;
            case Define.Object.Food:
                // ��� Ž����
                break;
            case Define.Object.Default:
                break;
        }
    }

    private Define.Object GetObjectFromTag(string tag)
    {
        switch (tag)
        {
            case "Table":
                return Define.Object.Table;
            case "Bin":
                return Define.Object.Bin;
            case "Crate":
                return Define.Object.Crate;
            case "PlateReturn":
                return Define.Object.PlateReturn;
            case "Passing":
                return Define.Object.Passing;
            case "CuttingBoard":
                return Define.Object.CuttingBoard;
            case "Food":
                return Define.Object.Food;
            default:
                return Define.Object.Default;
        }
    }
}
