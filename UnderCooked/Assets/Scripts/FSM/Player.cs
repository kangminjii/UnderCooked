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


    public CookingPlace Cook;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public Transform SpawnPos;
    public GameObject Knife;
    private Transform ChopPos;

    private float lastDashTime = 0f;
    private float dashCooldown = 0.3f;

    public Vector3 LookDir;
    public bool canCut;
    public bool FoodGrab;
    public float _speed = 5.0f;


    public delegate void ObjectSelectHandler(GameObject gameObject);
    public GameObject SelectObj = null;



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
        ChopPos = this.transform.Find("ChopPos");

        Overlap.ObjectSelectEnter += Select;
    }

    private void OnDestroy()
    {
        Overlap.ObjectSelectEnter -= Select;
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
        float dashForce = 7f;

        // 쿨타임 체크
        if (Time.time - lastDashTime >= dashCooldown)       
        {
            Transform DashPos = transform.Find("Chararcter");
            Rigidbody.velocity = LookDir * dashForce;
            Rigidbody.AddForce(LookDir * dashForce, ForceMode.Force);
            Managers.Resource.Instantiate("DashEffect", this.transform.position, Quaternion.identity, DashPos);
            Managers.Sound.Play("AudioClip/Dash5", Define.Sound.Effect);
            // 다시 쿨타임을 시작하기 위해 시간 기록
            lastDashTime = Time.time;
        }
    }


    // Chop 조건
    private void Select(GameObject Obj)
    {
        if (Obj != null)
        {
            SelectObj = Obj;
            CookingPlace place = Obj.GetComponent<CookingPlace>();

            if (place != null)
            {
                Transform spawnPos = place.transform.Find("SpawnPos");

                if (spawnPos.childCount == 1 && place.SliceFoodbool == false)
                    canCut = true;
                else
                    canCut = false;

                if (place.ChopCount > 0)
                    FoodGrab = false;
                else 
                    FoodGrab = true;
            }
            else
            {
                canCut = false;
            }
        }
        else
        {
            canCut = false;
            SelectObj = null;
        }
    }

    public void Cutting()
    {
        Cook = SelectObj.GetComponent<CookingPlace>();
        Cook.CuttingFood();

        Managers.Resource.Instantiate("Chophit", ChopPos.position, Quaternion.identity,ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
    }


}
