
using UnityEngine;

public class Player : StateMachine
{
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

    public Animator Animator;
    public Rigidbody Rigidbody;
    public GameObject Knife;
    public Transform SpawnPos;
    public Transform ChopPos;
    public bool CanCut;
    public bool CanGrab;
    public float Speed = 5.0f;

    public delegate void ObjectSelectHandler(GameObject gameObject);
    public GameObject SelectObj = null;


    float _lastDashTime = 0f;
    float _dashCoolDown = 0.3f;
    Vector3 LookDir;


    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        ChopState = new Chop(this);
        GrabIdleState = new Grab_Idle(this);
        GrabMovingState = new Grab_Moving(this);

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

        Rigidbody.position += moveDirection.normalized * Time.deltaTime * Speed;

        if (moveDirection != Vector3.zero)
            PlayerRotate(moveDirection);


        if (Input.GetKeyDown(KeyCode.LeftAlt))
            Dash();
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
        if (Time.time - _lastDashTime >= _dashCoolDown)       
        {
            Transform DashPos = transform.Find("Chararcter");
            Rigidbody.velocity = LookDir * dashForce;
            Rigidbody.AddForce(LookDir * dashForce, ForceMode.Force);
            Managers.Resource.Instantiate("DashEffect", this.transform.position, Quaternion.identity, DashPos);
            Managers.Sound.Play("AudioClip/Dash5", Define.Sound.Effect);
            // 다시 쿨타임을 시작하기 위해 시간 기록
            _lastDashTime = Time.time;
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

                if (spawnPos.childCount == 1 && place.CanChop == true)
                    CanCut = true;
                else
                    CanCut = false;

                if (place.ChopCount > 0)
                    CanGrab = false;
                else 
                    CanGrab = true;
            }
            else
            {
                CanCut = false;
            }
        }
        else
        {
            CanCut = false;
            SelectObj = null;
        }
    }

    
    // Chop할때 효과들
    public void Cutting()
    {
        CookingPlace cook = SelectObj.GetComponent<CookingPlace>();
        cook.CuttingFood();

        Managers.Resource.Instantiate("Chophit", ChopPos.position, Quaternion.identity,ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
    }


    // Idle & Moving에서 Object 탐지시
    public void InteractObject()
    {
        if (SelectObj == null)
            return;

        // Object == 접시
        if (SelectObj.tag == "PlateReturn")
        {
            PlateReturn plateReturn = SelectObj.GetComponent<PlateReturn>();

            if (plateReturn.CurrentPlateNumber > 0 && SpawnPos.childCount < 1)
            {
                if (plateReturn.PlateSpawnPos.childCount == 0)
                    return;

                Managers.Resource.Instantiate("Plate", SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                Managers.Resource.Destroy(plateReturn.PlateSpawnPos.GetChild(plateReturn.PlateSpawnPos.childCount - 1).gameObject);
            }
        }

        // Object == 재료상자
        if (SelectObj.tag == "Crate" && SpawnPos.childCount < 1)
        {
            Animator crateBoxAnimator = SelectObj.GetComponent<Animator>();
            crateBoxAnimator.SetTrigger("IsOpen");

            string ingredientName = SelectObj.transform.GetChild(0).name.Remove(0, "Crate_".Length);

            Managers.Resource.Instantiate(ingredientName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
        }

        // Object == 음식
        if (SelectObj.tag == "Food")
        {
            string droppedItemName = SelectObj.name.Replace("_Drop(Clone)", "");

            Managers.Resource.Instantiate(droppedItemName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
            Managers.Resource.Destroy(SelectObj);
        }

        // Object == 도마
        if (SelectObj.tag == "CuttingBoard" && CanGrab == false)
            return;

        // Object == 테이블
        if (SelectObj.transform.Find("SpawnPos") == null)
        {
            return;
        }
        else if (SelectObj.transform.Find("SpawnPos").childCount == 1)
        {
            Transform table = SelectObj.transform.Find("SpawnPos");
            string tableObjectName = table.GetChild(0).name.Replace("(Clone)", "");

            Managers.Resource.Instantiate(tableObjectName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
            Managers.Resource.Destroy(table.GetChild(0).gameObject);
        }
    }

}
