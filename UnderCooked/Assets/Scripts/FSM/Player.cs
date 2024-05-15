using System;
using UnityEngine;

public delegate void ObjectSelectHandler(GameObject obj);
public delegate void PlateReturnedHandler();


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

    public GameObject SelectObj = null;
    public static Action<string> FoodOrderCheck;


    float _lastDashTime = 0f;
    float _dashCoolDown = 0.3f;
    Vector3 _lookDir;
    
    // 구독 이벤트 발생 ( 옵저버 패턴)
    Overlap _overlap;


    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        ChopState = new Chop(this);
        GrabIdleState = new Grab_Idle(this);
        GrabMovingState = new Grab_Moving(this);

        _overlap = GetComponent<Overlap>();
        _overlap.OverlapHandler += new ObjectSelectHandler(Select);
    }


    public event PlateReturnedHandler PlateReturned;
    
    protected virtual void OnPlateReturned()
    {
        if (PlateReturned != null)
        {
            PlateReturned.Invoke();
        }
    }


    private void OnDestroy()
    {
        _overlap.OverlapHandler -= new ObjectSelectHandler(Select);
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
        _lookDir = transform.forward;
    }

    public void Dash()
    {
        float dashForce = 7f;

        // 쿨타임 체크
        if (Time.time - _lastDashTime >= _dashCoolDown)       
        {
            Transform DashPos = transform.Find("Chararcter");
            Rigidbody.velocity = _lookDir * dashForce;
            Rigidbody.AddForce(_lookDir * dashForce, ForceMode.Force);
            Managers.Resource.Instantiate("DashEffect", this.transform.position, Quaternion.identity, DashPos);
            Managers.Sound.Play("AudioClip/Dash5", Define.Sound.Effect);
            // 다시 쿨타임을 시작하기 위해 시간 기록
            _lastDashTime = Time.time;
        }
    }


    // Chop 조건
    private void Select(GameObject obj)
    {
        if (obj != null)
        {
            SelectObj = obj;
            CookingPlace place = obj.GetComponent<CookingPlace>();

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
    private void Cutting()
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
        

        switch (SelectObj.tag)
        {
            case "PlateReturn": // 접시
            {
                PlateReturn plateReturn = SelectObj.GetComponent<PlateReturn>();

                if (SpawnPos.childCount < 1)
                {
                    if (plateReturn.PlateSpawnPos.childCount == 0)
                        return;

                    Managers.Resource.Instantiate("Plate", SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                    Managers.Resource.Destroy(plateReturn.PlateSpawnPos.GetChild(plateReturn.PlateSpawnPos.childCount - 1).gameObject);
                }
            }
            break;

            case "Crate": // 재료상자
            {
                if(SpawnPos.childCount < 1)
                {
                    Animator crateBoxAnimator = SelectObj.GetComponent<Animator>();
                    crateBoxAnimator.SetTrigger("IsOpen");

                    string ingredientName = SelectObj.transform.GetChild(0).name.Remove(0, "Crate_".Length);

                    Managers.Resource.Instantiate(ingredientName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                }
            }
            break;

            case "Food": // 음식
            {
                string droppedItemName = SelectObj.name.Replace("_Drop(Clone)", "");

                Managers.Resource.Instantiate(droppedItemName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                Managers.Resource.Destroy(SelectObj);
            }
            break;

            case "CuttingBoard":  // 도마
            {
                if (CanGrab == false)
                {
                    return;
                }
            }
            break;
        }


        // Grab
        // 테이블
        Transform spawnPos = SelectObj.transform.Find("SpawnPos");

        if (spawnPos != null)
        {
            if(spawnPos.childCount == 1)
            {
                Transform table = SelectObj.transform.Find("SpawnPos");
                string tableObjectName = table.GetChild(0).name.Replace("(Clone)", "");

                Managers.Resource.Instantiate(tableObjectName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                Managers.Resource.Destroy(table.GetChild(0).gameObject);
            }
        }
    }


    public void InteractObjectWhileGrabbing()
    {
        string grabObjectName = this.SpawnPos.GetChild(0).name.Replace("(Clone)", "");

        if (SelectObj == null)
        {
            Animator.SetBool("Grab", false);

            Managers.Sound.Play("AudioClip/Grab_Off", Define.Sound.Effect);
            Managers.Resource.Instantiate(grabObjectName + "_Drop", SpawnPos.position, Quaternion.identity);
            Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
            return;
        }


        switch (SelectObj.tag)
        {
            case "Bin": // 쓰레기통
            {
                Transform trash = SelectObj.transform.Find("BinSpawnPos");

                Managers.Sound.Play("AudioClip/TrashCan", Define.Sound.Effect);
                Managers.Resource.Instantiate(grabObjectName, trash.position, Quaternion.identity, trash);
                Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                trash.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("binTrigger");

                if (SpawnPos.GetChild(0).name.Contains("Plate"))
                {
                    Animator.SetBool("Grab", false);

                    // Plate 생성 이벤트 발생
                    OnPlateReturned();
                }
            }
            break;

            case "Food": // 음식
            {
                Animator.SetBool("Grab", false);

                Managers.Resource.Instantiate(grabObjectName + "_Drop", SpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
            }
            break;

            case "Passing": // 음식 제출대
            {
                if (SpawnPos.GetChild(0).name.Contains("Plate"))
                {
                    Animator.SetBool("Grab", false);

                    // Plate 생성 이벤트 발생
                    OnPlateReturned();


                    string returnFoodName;

                    if (SpawnPos.GetChild(0).name.Contains("Prawn"))
                        returnFoodName = "Prawn";
                    else if (SpawnPos.GetChild(0).name.Contains("Fish"))
                        returnFoodName = "Fish";
                    else
                        returnFoodName = null;

                    FoodOrderCheck.Invoke(returnFoodName);

                    Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
                }
            }
            break;
        }


        // 테이블
        Transform table = SelectObj.transform.Find("SpawnPos");

        if (table != null)
        {
            //도마 위에 접시 안올라가게 함
            if (SelectObj.tag == "CuttingBoard" && table.childCount < 1 && SpawnPos.GetChild(0).tag.Contains("Plate"))
                return;

            if (table.childCount == 1)
            {
                // 테이블 접시 + 플레이어 음식
                if (table.GetChild(0).tag == "EmptyPlate" && SpawnPos.GetChild(0).tag == "SlicedFood")
                {
                    Managers.Resource.Instantiate(grabObjectName + "_Plate", table.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, table);
                    Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
                    Managers.Resource.Destroy(table.GetChild(0).gameObject);
                }
                // 테이블 음식 + 플레이어 접시
                else if (SpawnPos.GetChild(0).tag == "EmptyPlate" && table.GetChild(0).tag == "SlicedFood")
                {
                    string tableObjectName = table.GetChild(0).name.Replace("(Clone)", "");

                    Managers.Resource.Instantiate(tableObjectName + "_Plate", SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                    Managers.Resource.Destroy(table.GetChild(0).gameObject);
                    Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                    Managers.Sound.Play("AudioClip/Grab_On", Define.Sound.Effect);
                }
            }
            else if (table.childCount < 1)
            {
                Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                if (grabObjectName == "Fish")
                    Managers.Resource.Instantiate(grabObjectName, table.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, table);
                else
                    Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);
            }
        }
    }

}
