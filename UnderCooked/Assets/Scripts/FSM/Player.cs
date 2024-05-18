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
     * Player�� ���µ�
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
     * Player(������ ������ ��ü)�� ������ �̺�Ʈ ��� 
     */
    public event PlateReturnHandler  PlateReturned;
    public event CookingPlaceHandler Cooking;
    public static Action<string>     FoodOrderCheck;


    /*
     * Player ���� ��ü �ʱ�ȭ
     * 
     * Object�� �����ϴ� Seeking Ŭ���� ����
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
     * .NET �̺�Ʈ �߻��� �����Լ��� �ַ� �����
     * -> �̺�Ʈ�� ��ϵ� ��� delegate�� ȣ���Ѵ�
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
     * Player FSM�� �ʱ� ���¸� Idle�� ������
     */
    protected override BaseState GetInitialState()
    {
        return IdleState;
    }


    /*
     * Player ������
     * -> Ű�Է¿� ���� ��ġ, ������ �ٲ�
     * -> Ư��Ű�� �뽬�� ����ϸ� ��Ÿ�ӿ� ���� �ٶ󺸴� �������� Rigidbody ���� ������
     * -> ����Ʈ ���� �� ���� ���
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

            // ��Ÿ�� üũ
            if (Time.time - _lastDashTime >= _dashCoolDown)
            {
                Rigidbody.velocity = _lookDir * dashForce;
                Rigidbody.AddForce(_lookDir * dashForce, ForceMode.Force);

                Managers.Resource.Instantiate("DashEffect", this.transform.position, Quaternion.identity, transform.Find("Chararcter"));
                Managers.Sound.Play("AudioClip/Dash5", Define.Sound.Effect);

                // �ٽ� ��Ÿ���� �����ϱ� ���� �ð� ���
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
     * Player�� Object�� �浹�� �� �ҷ����� �Լ�
     * 
     * ���� Tag�� ������ �� CookingPlace���� ������ �̺�Ʈ �߻�
     * -> ���ǿ� ���� Chop ���¿� Grab ���¸� ������Ʈ
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
     * Chop �ִϸ��̼� �߻��� ȣ��Ǵ� Animation Event
     * -> Chop�� Ƚ���� ī��Ʈ�� �� �ʿ�
     */
    private void Cutting()
    {
        if(SelectObj != null)
        {
            CookingPlace place = SelectObj.GetComponent<CookingPlace>();
            place.HandleChopCounting();
        }
    }


    // ��ĥ�κ�*******************************
    // Idle & Moving���� Object Ž����
    public void InteractObject()
    {
        if (SelectObj == null)
            return;
        

        switch (SelectObj.tag)
        {
            case "PlateReturn": // ����
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

            case "Crate": // ������
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

            case "Food": // ����
            {
                string droppedItemName = SelectObj.name.Replace("_Drop(Clone)", "");

                Managers.Resource.Instantiate(droppedItemName, SpawnPos.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, SpawnPos);
                Managers.Resource.Destroy(SelectObj);
            }
            break;

            case "CuttingBoard":  // ����
            {
                if (SpawnPos.childCount > 0)
                {
                    // Chop ���� �̺�Ʈ �߻�
                    //OnCooking();
                }
            }
            break;
        }


        // Grab
        // ���̺�
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

        // �ٴڿ� ��ü�� ����߸� ��
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
            case "Bin": // ��������
            {
                Transform trash = SelectObj.transform.Find("BinSpawnPos");

                Managers.Sound.Play("AudioClip/TrashCan", Define.Sound.Effect);
                Managers.Resource.Instantiate(grabObjectName, trash.position, Quaternion.identity, trash);
                Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);

                trash.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("binTrigger");

                if (SpawnPos.GetChild(0).name.Contains("Plate"))
                {
                    Animator.SetBool("Grab", false);

                    // Plate ���� �̺�Ʈ �߻�
                    OnPlateReturned();
                }
            }
            break;

            case "Food": // ����
            {
                Debug.Log("Food �϶�");
                Animator.SetBool("Grab", false);

                Managers.Resource.Instantiate(grabObjectName + "_Drop", SpawnPos.position, Quaternion.identity);
                Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
            }
            break;

            case "Passing": // ���� �����
            {
                if (SpawnPos.GetChild(0).name.Contains("Plate"))
                {
                    Animator.SetBool("Grab", false);

                    // Plate ���� �̺�Ʈ �߻�
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


        // ���̺�
        Transform table = SelectObj.transform.Find("SpawnPos");

        if (table != null)
        {
            //���� ���� ���� �ȿö󰡰� ��
            if (SelectObj.tag == "CuttingBoard")
            { 
                if (table.childCount < 1 && SpawnPos.GetChild(0).tag.Contains("Plate"))
                    return;
            }

            if (table.childCount == 1)
            {
                // ���̺� ���� + �÷��̾� ����
                if (table.GetChild(0).tag == "EmptyPlate" && SpawnPos.GetChild(0).tag == "SlicedFood")
                {
                    Managers.Resource.Instantiate(grabObjectName + "_Plate", table.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity, table);
                    Managers.Resource.Destroy(SpawnPos.GetChild(0).gameObject);
                    Managers.Resource.Destroy(table.GetChild(0).gameObject);
                }
                // ���̺� ���� + �÷��̾� ����
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
