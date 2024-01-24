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


    public CookingPlace Doma;

    public Animator Anim;
    public Rigidbody Rigidbody;

    public GameObject Knife;
    public GameObject PlayerPrawn;
    public Transform SpawnPoint;

    public Vector3 LookDir;
    public float DashCoolDown = 0.6f;
    public float LstDashTime = -Mathf.Infinity;

    public bool Cutting = false;

    
    GameObject _lastTriggeredObject;
    GameObject _triggerExitObject;
    

    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        ChopState = new Chop(this);
        GrabIdleState = new Grab_Idle(this);
        GrabMovingState = new Grab_Moving(this);


        Rigidbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        SpawnPoint = this.transform.Find("SpawnPoint");

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
            Doma = null;
        }
    }

    private void HandleObjectTriggerEnter(GameObject triggeredObject)
    {
        // �ٸ� Table�� Trigger�ɶ�
        if (_lastTriggeredObject != null && triggeredObject.name != _lastTriggeredObject.name)
        {
            Searching interactingObject = triggeredObject.GetComponent<Searching>();
            interactingObject.EnableColor();

            if (_lastTriggeredObject != null)
            {
                // TriggerExit���� �Ȳ��� Object ó��
                Searching pastObject = _lastTriggeredObject.GetComponent<Searching>();
                pastObject.DisableColor();
            }
        }
        // ���� Table�� Trigger�ɶ�
        else
        {
            if (_triggerExitObject == triggeredObject)
            {
                Searching interactingObject = triggeredObject.GetComponent<Searching>();
                interactingObject.EnableColor();
            }
        }

        _lastTriggeredObject = triggeredObject;
    }

    private void HandleObjectTriggerExit(GameObject triggeredObject)
    {
        Searching interactingObject = triggeredObject.GetComponent<Searching>();
        interactingObject.DisableColor();

        _triggerExitObject = triggeredObject;
    }


    private void TriggeredObject(GameObject triggeredObject)
    {
        Define.Object objectType = GetObjectFromTag(triggeredObject.tag);

        switch (objectType)
        {
            case Define.Object.Table:
                Debug.Log("Table����");
                break;
            case Define.Object.Bin:
                Debug.Log("Bin����");
                break;
            case Define.Object.Crate:
                Debug.Log("Crate����");
                break;
            case Define.Object.PlateReturn:
                Debug.Log("PlateReturn����");
                break;
            case Define.Object.Passing:
                Debug.Log("Passing����");
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
            default:
                return Define.Object.Default;
        }
    }
}
