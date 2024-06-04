using System;
using UnityEngine;


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
    public static Action         Cooking;
    public static Action         ChopCounting;
    public static Action         PlateGenerate;
    public static Action         PlateDestroy;
    public static Action<string> FoodOrderCheck;


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
     * Player가 Object와 충돌될 때 호출되는 함수
     * -> 도마 Tag를 감지할 때 CookingPlace에서 구독한 이벤트 발생
     *  -> 조건에 따라 Chop 상태와 Grab 상태를 업데이트
     */
    private void Select(GameObject obj)
    {
        SelectObj = obj;
        CanCut = false;

        if(obj != null)
        {
            if (obj.CompareTag("CuttingBoard"))
            {
                Cooking.Invoke();

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
     * -> CookingPlace에서 구독한 이벤트 발생
     *  -> Chop한 횟수를 카운트할 때 필요
     * 
     * -> Chop할 때 나오는 이펙트 생성 및 사운드 재생
     */
    private void Cutting()
    {
        ChopCounting.Invoke();

        Managers.Resource.Instantiate("Chophit", ChopPos.position, Quaternion.identity, ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
    }


    /*
     * Idle & Moving에서 Object와 상호작용할 때 호출되는 함수
     * -> Player가 탐지한 SelectObj의 tag에 따라 행동이 나뉨
     *  
     *  -> PlateGenerator(접시 생성대) : 접시를 잡는 용도
     *      -> Player 손 위에 접시 생성, 접시 생성대에 있는 접시 파괴
     *      
     *  -> Crate(재료 상자) : 재료를 잡는 용도
     *      -> 애니메이션 재생, 재료 상자 이름에 따라 Player 손 위에 재료 생성
     *      
     *  -> Food(음식) : 땅에 떨어진 음식을 잡는 용도
     *      -> Player 손 위에 떨어진 음식 이름에 따라 생성, 땅에 있는 음식 파괴
     *      
     *  -> Table(식탁) : 식탁 위에 있는 Object를 잡는 용도
     *      -> Player가 잡을 수 있는 상태의 Object만 손 위에 생성, 식탁에 있는 Object 파괴
     *  
     *  -> CuttingBoard(도마) : 도마 위에 있는 Object를 잡는 용도
     *      -> Table과 같으며 썰리지 않았을 때만 잡을 수 있는 조건 추가
     */
    public void InteractObject()
    {
        if (SelectObj == null)
            return;
        
        switch (SelectObj.tag)
        {
            case "PlateGenerator":
            {
                if (SelectObj.transform.GetChild(0).childCount == 0)
                    return;

                PlateDestroy.Invoke();
                Managers.Resource.Instantiate("Plate", SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
            }
            break;

            case "Crate":
            {
                SelectObj.GetComponent<Animator>().SetTrigger("IsOpen");

                string ingredientName = SelectObj.transform.GetChild(0).name.Remove(0, "Crate_".Length);

                Managers.Resource.Instantiate(ingredientName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
            }
            break;

            case "Food":
            {
                string droppedItemName = SelectObj.name.Replace("_Drop(Clone)", "");

                Managers.Resource.Instantiate(droppedItemName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                Managers.Resource.Destroy(SelectObj);
            }
            break;

            case "Table":
            {
                Transform tableSpawnPos = SelectObj.transform.GetChild(0);

                if (tableSpawnPos != null)
                {
                    if (tableSpawnPos.childCount == 1)
                    {
                        string tableObjectName = tableSpawnPos.GetChild(0).name.Replace("(Clone)", "");

                        Managers.Resource.Instantiate(tableObjectName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                        
                            // 고치기
                            Managers.Resource.Destroy(tableSpawnPos.GetChild(0).gameObject);
                    }
                }
            }
            break;

            case "CuttingBoard":
            {
                Transform tableSpawnPos = SelectObj.transform.GetChild(0);

                if (tableSpawnPos != null)
                {
                    if (CanGrab && tableSpawnPos.childCount == 1)
                    {
                        string tableObjectName = tableSpawnPos.GetChild(0).name.Replace("(Clone)", "");

                        Managers.Resource.Instantiate(tableObjectName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);

                            // 고치기
                            Managers.Resource.Destroy(tableSpawnPos.GetChild(0).gameObject);
                    }
                }
            }
            break;
        }
    }


    /*
     * Grab_Idle & Grab_Moving에서 Object와 상호작용할 때 호출되는 함수
     * -> Player가 탐지한 SelectObj의 tag에 따라 행동이 나뉨
     *  
     *  -> null & Food(음식) : 들고 있는 Object를 바닥에 떨어뜨리는 용도
     *  
     *  -> Bin(쓰레기통) : 들고 있는 Object를 버리는 용도
     *      -> 접시를 들고 있을 때 접시 생성 이벤트 발생
     *      -> 잡고 있는 Object 파괴
     *      -> 쓰레기통 자식으로 생성, 쓰레기통 애니메이션 재생
     *  
     *  -> Passing(반납대) : 들고 있는 Object를 제출하는 용도
     *      -> 접시가 있어야만 조건 검사를 함
     *      -> 접시 생성 이벤트 호출
     *      -> 잡고 있는 Object 파괴
     *      -> 음식 이름에 따라 주문서 확인 이벤트 호출하고 음식 이름을 인자로 넘김
     *  
     *  -> CuttingBoard(도마) : 들고 있는 Object가 도마에 올라갈 수 있는지 판단하는 용도
     *      -> 도마에 물체가 있을 때
     *          -> Player가 접시를 들고 도마에 있는 Object를 잡을 수 있을 때
     *              -> 두 Object를 파괴 후 Player에게 완성된 음식을 생성
     *      -> 도마에 물체가 없을 때
     *          -> 접시는 못 올리게 막음
     *          -> "생선" Object만 높이 조절 한 후 도마에 배치
     *          -> 잡고 있는 Object 파괴
     *  
     *  -> Table(조리대)
     *      -> 테이블에 물체가 있을 때
     *          -> 테이블에 접시가 있고 Player가 접시에 담을 수 있는 Object를 잡고 있을 때
     *              -> 두 Object 파괴 후 Table에 완성된 음식을 생성
     *          -> 테이블에 완성된 음식이 있고 Player가 접시를 잡고 있을 때
     *              -> 두 Object 파괴 후 Player에게 완성된 음식을 생성
     *      -> 테이블에 물체가 없을 때
     *          -> "생선" Object만 높이 조절 한 후 도마에 배치
     *          -> 잡고 있는 Object 파괴
     */
    public void InteractObjectWhileGrabbing()
    {
        string grabObjectName = SpawnPos.GetChild(0).name.Replace("(Clone)", "");

        if (SelectObj == null || SelectObj.tag == "Food")
        {
            Animator.SetBool("Grab", false);

            Managers.Sound.Play("AudioClip/Grab_Off", Define.Sound.Effect);
            Managers.Resource.Instantiate(grabObjectName + "_Drop", SpawnPos.position, Quaternion.identity);
            Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
            return;
        }

        switch (SelectObj.tag)
        {
            case "Bin":
            {
                Transform trash = SelectObj.transform.GetChild(0);
                
                if (SpawnPos.GetChild(0).name.Contains("Plate"))
                {
                     PlateGenerate.Invoke();
                }

                Managers.Sound.Play("AudioClip/TrashCan", Define.Sound.Effect);
                Managers.Resource.Instantiate(grabObjectName, trash.position, Quaternion.identity, trash);
                Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                trash.GetChild(0).GetComponent<Animator>().SetTrigger("binTrigger");
            }
            break;

            case "Passing":
            {
                if (SpawnPos.GetChild(0).name.Contains("Plate"))
                {
                    PlateGenerate.Invoke();

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

            case "CuttingBoard":
            {
                Transform table = SelectObj.transform.GetChild(0);

                if(table != null)
                {
                    if (table.childCount == 1)
                    {
                        if (SpawnPos.GetChild(0).tag == "EmptyPlate" && CanGrab)
                        {
                            string tableObjectName = table.GetChild(0).name.Replace("(Clone)", "");

                            Managers.Resource.Instantiate(tableObjectName + "_Plate", SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                            Managers.Resource.Destroy(table.GetChild(0).gameObject);
                            Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                            Managers.Sound.Play("AudioClip/Grab_On", Define.Sound.Effect);
                        }
                    }
                    else
                    {
                        if (SpawnPos.GetChild(0).tag.Contains("Plate"))
                            return;

                        if (grabObjectName == "Fish")
                            Managers.Resource.Instantiate(grabObjectName, table.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, table);
                        else
                            Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);
                        
                        Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
                    }
                }
            }
            break;

            case "Table":
            {
                Transform table = SelectObj.transform.GetChild(0);

                if(table != null)
                {
                    if (table.childCount == 1)
                    {
                        if (table.GetChild(0).tag == "EmptyPlate" && SpawnPos.GetChild(0).tag == "SlicedFood")
                        {
                            Managers.Resource.Instantiate(grabObjectName + "_Plate", table.position, Quaternion.identity, table);
                            Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
                            Managers.Resource.Destroy(table.GetChild(0).gameObject);
                        }
                        else if (SpawnPos.GetChild(0).tag == "EmptyPlate" && table.GetChild(0).tag == "SlicedFood")
                        {
                            string tableObjectName = table.GetChild(0).name.Replace("(Clone)", "");

                            Managers.Resource.Instantiate(tableObjectName + "_Plate", SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                            Managers.Resource.Destroy(table.GetChild(0).gameObject);
                            Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                            Managers.Sound.Play("AudioClip/Grab_On", Define.Sound.Effect);
                        }
                    }
                    else
                    {
                        if (grabObjectName == "Fish")
                            Managers.Resource.Instantiate(grabObjectName, table.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, table);
                        else
                            Managers.Resource.Instantiate(grabObjectName, table.position, Quaternion.identity, table);
                        
                            Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
                    }
                }
            }
            break;
        }
    }
}
