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
    public static Action         Cooking;
    public static Action         ChopCounting;
    public static Action         PlateGenerate;
    public static Action         PlateDestroy;
    public static Action<string> FoodOrderCheck;


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
     * Player�� Object�� �浹�� �� ȣ��Ǵ� �Լ�
     * -> ���� Tag�� ������ �� CookingPlace���� ������ �̺�Ʈ �߻�
     *  -> ���ǿ� ���� Chop ���¿� Grab ���¸� ������Ʈ
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
     * Chop �ִϸ��̼� �߻��� ȣ��Ǵ� Animation Event
     * -> CookingPlace���� ������ �̺�Ʈ �߻�
     *  -> Chop�� Ƚ���� ī��Ʈ�� �� �ʿ�
     * 
     * -> Chop�� �� ������ ����Ʈ ���� �� ���� ���
     */
    private void Cutting()
    {
        ChopCounting.Invoke();

        Managers.Resource.Instantiate("Chophit", ChopPos.position, Quaternion.identity, ChopPos);
        Managers.Sound.Play("AudioClip/Chop_Sound", Define.Sound.Effect);
    }


    /*
     * Idle & Moving���� Object�� ��ȣ�ۿ��� �� ȣ��Ǵ� �Լ�
     * -> Player�� Ž���� SelectObj�� tag�� ���� �ൿ�� ����
     *  
     *  -> PlateGenerator(���� ������) : ���ø� ��� �뵵
     *      -> Player �� ���� ���� ����, ���� �����뿡 �ִ� ���� �ı�
     *      
     *  -> Crate(��� ����) : ��Ḧ ��� �뵵
     *      -> �ִϸ��̼� ���, ��� ���� �̸��� ���� Player �� ���� ��� ����
     *      
     *  -> Food(����) : ���� ������ ������ ��� �뵵
     *      -> Player �� ���� ������ ���� �̸��� ���� ����, ���� �ִ� ���� �ı�
     *      
     *  -> Table(��Ź) : ��Ź ���� �ִ� Object�� ��� �뵵
     *      -> Player�� ���� �� �ִ� ������ Object�� �� ���� ����, ��Ź�� �ִ� Object �ı�
     *  
     *  -> CuttingBoard(����) : ���� ���� �ִ� Object�� ��� �뵵
     *      -> Table�� ������ �丮�� �ʾ��� ���� ���� �� �ִ� ���� �߰�
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
                        
                            // ��ġ��
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

                            // ��ġ��
                            Managers.Resource.Destroy(tableSpawnPos.GetChild(0).gameObject);
                    }
                }
            }
            break;
        }
    }


    /*
     * Grab_Idle & Grab_Moving���� Object�� ��ȣ�ۿ��� �� ȣ��Ǵ� �Լ�
     * -> Player�� Ž���� SelectObj�� tag�� ���� �ൿ�� ����
     *  
     *  -> null & Food(����) : ��� �ִ� Object�� �ٴڿ� ����߸��� �뵵
     *  
     *  -> Bin(��������) : ��� �ִ� Object�� ������ �뵵
     *      -> ���ø� ��� ���� �� ���� ���� �̺�Ʈ �߻�
     *      -> ��� �ִ� Object �ı�
     *      -> �������� �ڽ����� ����, �������� �ִϸ��̼� ���
     *  
     *  -> Passing(�ݳ���) : ��� �ִ� Object�� �����ϴ� �뵵
     *      -> ���ð� �־�߸� ���� �˻縦 ��
     *      -> ���� ���� �̺�Ʈ ȣ��
     *      -> ��� �ִ� Object �ı�
     *      -> ���� �̸��� ���� �ֹ��� Ȯ�� �̺�Ʈ ȣ���ϰ� ���� �̸��� ���ڷ� �ѱ�
     *  
     *  -> CuttingBoard(����) : ��� �ִ� Object�� ������ �ö� �� �ִ��� �Ǵ��ϴ� �뵵
     *      -> ������ ��ü�� ���� ��
     *          -> Player�� ���ø� ��� ������ �ִ� Object�� ���� �� ���� ��
     *              -> �� Object�� �ı� �� Player���� �ϼ��� ������ ����
     *      -> ������ ��ü�� ���� ��
     *          -> ���ô� �� �ø��� ����
     *          -> "����" Object�� ���� ���� �� �� ������ ��ġ
     *          -> ��� �ִ� Object �ı�
     *  
     *  -> Table(������)
     *      -> ���̺� ��ü�� ���� ��
     *          -> ���̺� ���ð� �ְ� Player�� ���ÿ� ���� �� �ִ� Object�� ��� ���� ��
     *              -> �� Object �ı� �� Table�� �ϼ��� ������ ����
     *          -> ���̺� �ϼ��� ������ �ְ� Player�� ���ø� ��� ���� ��
     *              -> �� Object �ı� �� Player���� �ϼ��� ������ ����
     *      -> ���̺� ��ü�� ���� ��
     *          -> "����" Object�� ���� ���� �� �� ������ ��ġ
     *          -> ��� �ִ� Object �ı�
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
