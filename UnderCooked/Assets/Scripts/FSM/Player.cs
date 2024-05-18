using System;
using UnityEngine;

public delegate void PlateReturnHandler();
public delegate void CookingPlaceHandler();


public class Player : StateMachine
{
    float   _lastDashTime = 0f;
    float   _dashCoolDown = 0.3f;
    Vector3 _lookDir;
    Seeking _seeking;


    public Animator   Animator;
    public Rigidbody  Rigidbody;
    public GameObject Knife;
    public GameObject SelectObj;
    public Transform  SpawnPos;
    public Transform  ChopPos;
    public bool       CanCut;
    public bool       CanGrab;
    public float      Speed = 5.0f;


    /*
     * Player의 상태들
     */
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


    /*
     * Player(옵저버 패턴의 주체)가 구독한 이벤트 목록 
     */
    public event PlateReturnHandler  PlateReturned;
    public event CookingPlaceHandler Cooking;
    public static Action<string>     FoodOrderCheck;


    /*
     * Player 상태 객체 초기화
     * 
     * Object를 감지하는 Seeking 클래스 구독
     */
    private void Awake()
    {
        IdleState = new Idle(this);
        MovingState = new Moving(this);
        ChopState = new Chop(this);
        GrabIdleState = new Grab_Idle(this);
        GrabMovingState = new Grab_Moving(this);

        _seeking = GetComponent<Seeking>();
        
        if(_seeking != null)
        {
            _seeking.Seek += Select;
        }
    }

    private void OnDestroy()
    {
        if (_seeking != null)
        {
            _seeking.Seek -= Select;
        }
    }


    /*
     * .NET 이벤트 발생은 가상함수를 주로 사용함
     * -> 이벤트에 등록된 모든 delegate를 호출한다
     */
    protected virtual void OnPlateReturned()
    {
        if (PlateReturned != null)
        {
            PlateReturned.Invoke();
        }
    }


    protected virtual void OnCooking()
    {
        if (Cooking != null)
        {
            Cooking.Invoke();
        }
    }


    /*
     * Player FSM의 초기 상태를 Idle로 정의함
     */
    protected override BaseState GetInitialState()
    {
        return IdleState;
    }


    /*
     * Player 움직임
     * -> 키입력에 따라 위치, 방향이 바뀜
     * -> 특정키로 대쉬를 사용하면 쿨타임에 따라 바라보는 방향으로 Rigidbody 힘이 가해짐
     * -> 이펙트 생성 및 사운드 출력
     */
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
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            float dashForce = 7f;

            // 쿨타임 체크
            if (Time.time - _lastDashTime >= _dashCoolDown)
            {
                Rigidbody.velocity = _lookDir * dashForce;
                Rigidbody.AddForce(_lookDir * dashForce, ForceMode.Force);

                Managers.Resource.Instantiate("DashEffect", this.transform.position, Quaternion.identity, transform.Find("Chararcter"));
                Managers.Sound.Play("AudioClip/Dash5", Define.Sound.Effect);

                // 다시 쿨타임을 시작하기 위해 시간 기록
                _lastDashTime = Time.time;
            }
        }

        Rigidbody.position += moveDirection.normalized * Time.deltaTime * Speed;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.06f);
            _lookDir = transform.forward;
        }
    }


    /*
     * Player가 Object와 충돌될 때 불러지는 함수
     * 
     * 도마 Tag를 감지할 때 CookingPlace에서 구독한 이벤트 발생
     * -> 조건에 따라 Chop 상태와 Grab 상태를 업데이트
     */
    private void Select(GameObject obj)
    {
        SelectObj = obj;
        CanCut = false;

        if(obj != null)
        {
            if (obj.CompareTag("CuttingBoard"))
            {
                OnCooking();
            
                CookingPlace place = obj.GetComponent<CookingPlace>();

                if (place.CanChop == true)
                    CanCut = true;
                else
                    CanCut = false;

                if (place.ChopCount > 0)
                    CanGrab = false;
                else 
                    CanGrab = true;
            }
        }
    }


    /*
     * Chop 애니메이션 발생시 호출되는 Animation Event
     * -> Chop한 횟수를 카운트할 때 필요
     */
    private void Cutting()
    {
        if(SelectObj != null)
        {
            CookingPlace place = SelectObj.GetComponent<CookingPlace>();
            place.HandleChopCounting();
        }
    }


    // 고칠부분*******************************
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
                if (SpawnPos.childCount < 1)
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
                if (SpawnPos.childCount > 0)
                {
                    // Chop 가능 이벤트 발생
                    //OnCooking();
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

        // 바닥에 물체를 떨어뜨릴 때
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
                Debug.Log("Food 일때");
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
            if (SelectObj.tag == "CuttingBoard")
            { 
                if (table.childCount < 1 && SpawnPos.GetChild(0).tag.Contains("Plate"))
                    return;
            }

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
