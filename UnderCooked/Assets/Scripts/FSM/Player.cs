using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : StateMachine
{
    // 상태
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
    public float _speed = 5.0f;


    public delegate void ObjectSelectHandler(GameObject gameObject);
    public static event ObjectSelectHandler ObjectSelectEnter;

    public GameObject SelectObj = null; // 선택된 물체



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

        Overlap.ObjectSelectEnter -= Select;
        Overlap.ObjectSelectEnter += Select;


    }


    protected override BaseState GetInitialState()
    {
        return IdleState;
    }

    // Player 움직임
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






        // !TODO : 오브젝트가 들어왔을 때 로직을 작성
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
