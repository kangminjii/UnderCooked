using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    [SerializeField] Overlap overlap;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public Transform SpawnPos;
    public GameObject Knife;

    public Vector3 LookDir;
    public bool canCut;

    //public GameObject EnterTriggeredObject = null; // �������� ��ü
    //public GameObject ExitTriggeredObject = null;   // �������� ��ü


    public delegate void ObjectSelectHandler(GameObject gameObject);
    public static event ObjectSelectHandler ObjectSelectEnter;

    public GameObject SelectObj = null; // ���õ� ��ü

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

        //Searching.ObjectTriggerEnter -= HandleObjectTriggerEnter;
        //Searching.ObjectTriggerExit -= HandleObjectTriggerExit;
        //Searching.ObjectTriggerEnter += HandleObjectTriggerEnter;
        //Searching.ObjectTriggerExit += HandleObjectTriggerExit;


        Overlap.ObjectSelectEnter -= Select;
        Overlap.ObjectSelectEnter += Select;


    }


    protected override BaseState GetInitialState()
    {
        return IdleState;
    }

    private void Select(GameObject Obj)
    {
        //SelectObj = null;
        if(Obj != null)
        {
            SelectObj = Obj;
            
        }
        else
        {
           // Debug.Log("Exit");
            canCut = false;
            SelectObj = null;
            return;
        }
        //Debug.Log(Obj.name);
        CookingPlace place = Obj.GetComponent<CookingPlace>();
        if (place != null)
            canCut = true;


        //if (Obj == null)
        //{
        //    Debug.Log("Exit");
        //    canCut = false;
        //    SelectObj = null;
        //    return;
        //}






        // !TODO : ������Ʈ�� ������ �� ������ �ۼ�
    }

   
    //private void HandleObjectTriggerEnter(GameObject triggeredObject)
    //{
    //    TriggeredObject(triggeredObject);

    //    // �ٸ� Table�� Trigger�ɶ�
    //    if (EnterTriggeredObject != null)
    //    {
    //        if(triggeredObject.name != EnterTriggeredObject.name)
    //        {
    //            Searching interactingObject = triggeredObject.GetComponent<Searching>();
    //            interactingObject.EnableColor();

    //            // TriggerExit���� �Ȳ��� Object ó��
    //            Searching pastObject = EnterTriggeredObject.GetComponent<Searching>();
    //            pastObject.DisableColor();
    //        }
    //    }
    //    // ���� Table�� Trigger�ɶ�
    //    else
    //    {
    //        if (ExitTriggeredObject == triggeredObject)
    //        {
    //            Searching interactingObject = triggeredObject.GetComponent<Searching>();
    //            interactingObject.EnableColor();
    //        }
    //    }

    //    EnterTriggeredObject = triggeredObject;
    //}

    //private void HandleObjectTriggerExit(GameObject triggeredObject)
    //{
    //    TriggeredObjectExit(triggeredObject);

    //    Searching interactingObject = triggeredObject.GetComponent<Searching>();
    //    interactingObject.DisableColor();

    //    ExitTriggeredObject = triggeredObject;
    //}

    //private void TriggeredObjectExit(GameObject triggeredObject)
    //{
    //    Define.Object objectType = GetObjectFromTag(triggeredObject.tag);

    //    switch (objectType)
    //    {
    //        case Define.Object.CuttingBoard:
    //            canCut = false;
    //            break;
    //        case Define.Object.Default:
    //            break;
    //    }
    //}

    //private void TriggeredObject(GameObject triggeredObject)
    //{
    //    Define.Object objectType = GetObjectFromTag(triggeredObject.tag);

    //    switch (objectType)
    //    {
    //        case Define.Object.CuttingBoard:
    //            canCut = true;
    //            break;
    //        case Define.Object.Food:
    //            // ��� Ž����
    //            break;
    //        case Define.Object.Default:
    //            break;
    //    }
    //}

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
