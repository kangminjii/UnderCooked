using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [HideInInspector]
    public Idle IdleState;
    [HideInInspector]
    public Moving MovingState;
    [HideInInspector]
    public Dash DashState;
    [HideInInspector]
    public Chop ChopState;

    public CookingPlace Doma;

    public Animator Anim;
    public Rigidbody Rigidbody;
    public GameObject Knife;

    public Vector3 LookDir;
    public float DashCoolDown = 0.6f;
    public float LastDashTime = -Mathf.Infinity;

    public bool Cutting = false;

    private string _triggeredName = "";
    private string _triggerExitName = "";


    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        DashState = new Dash(this);
        ChopState = new Chop(this);

        //AddState(idleState, StateName.Idle);
        //AddState(movingState, StateName.Walk);
        //AddState(dashState, StateName.Dash);

        Rigidbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();


        Searching.ObjectTriggerEnter += HandleObjectTriggerEnter;
        Searching.ObjectTriggerExit += HandleObjectTriggerExit;

    }

    protected override BaseState GetInitialState()
    {
        return IdleState;
    }

    public void CheckDoma(Transform target)
    {

        if (Doma != target)
        {
            Cutting = false;
        }
    }

    private void HandleObjectTriggerEnter(string name, GameObject triggeredObject)
    {
        Debug.Log("����: " + name + ", ����: " + _triggeredName + ", Exit: " + _triggerExitName);
        
        // �ٸ� Table�� Trigger�ɶ�
        if(name != _triggeredName)
        {
            Searching interactingObject = GameObject.Find(name).GetComponent<Searching>();
            interactingObject.EnableColor();

            if (_triggeredName.Length > 0)
            {
                // TriggerExit���� �Ȳ��� Object ó��
                Searching pastObject = GameObject.Find(_triggeredName).GetComponent<Searching>();
                pastObject.DisableColor();
            }
        }
        // ���� Table�� Trigger�ɶ�
        else
        {
            if (name == _triggerExitName)
            {
                Searching interactingObject = GameObject.Find(name).GetComponent<Searching>();
                interactingObject.EnableColor();
            }
        }

        _triggeredName = name;

        TriggeredObject(triggeredObject);
    }

    private void HandleObjectTriggerExit(string name, GameObject triggeredObject)
    {
        Searching interactingObject = GameObject.Find(name).GetComponent<Searching>();
        interactingObject.DisableColor();

        _triggerExitName = name;
    }


    private void TriggeredObject(GameObject triggeredObject)
    {
        //if(triggeredObject.tag == "c")
        {

        }

       
    }

}
