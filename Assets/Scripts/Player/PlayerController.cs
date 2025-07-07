using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    //[field: SerializeField] public float AttackCoolTime { get; private set; }
    public Transform handTransform;
    [HideInInspector] GameObject onHandInstance;
    PlayerManager pm;
    DataManager dm;
    [field: SerializeField] public PlayerStatus Status { get; private set; }

    public PlayerView View { get; private set; }
    public ColliderController Cc { get; private set; }

    [HideInInspector] public Vector3 InputDir { get; private set; }
    [HideInInspector] public Vector2 MouseInputDir { get; private set; }

    [HideInInspector] public CinemachineVirtualCamera[] TPS_Cameras;
    int currentZoomIndex = 1;

    //private InputAction AimingAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction attackAction;
    private InputAction freeCamAction;
    private InputAction interactAction;
    private InputAction inventoryOpenAction;
    //private InputAction inventoryOffAction;
    private InputAction quickSlotActions;
    private InputAction escActions;


    //Vector3 moveDir;

    bool isMoveInput;
    bool isSprintInput;
    bool isJumpInput;
    bool isCrouchToggle;
    bool isAimingInput;
    bool isFreeCamModInput;
    bool isAttackInput;

    private Vector2 SmoothDir;      // ĳ���Ͱ� ������ �� ����
    private float SmoothTime = 0.1f; // ���� �ӵ�
    private Vector2 smoothVelocity; // SmoothDamp�� ���� ����

    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

    public bool jumpCooling;
    public bool isSprintJump;
    public bool isSprinting;

    public bool IsCurrentState(PlayerStateTypes state)
    {
        return stateMachine.CurState == stateMachine.stateDic[state];
    }

    private void Awake() => Init();


    private void Update()
    {
        ///////////////////////////
        /// �׽�Ʈ
        /// 
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(10, transform);
        }
        ///
        /////////////////////////////

        HandleSight(); // ȭ�� ȸ���� isControllLocked�� ���� �����ο�


        UpdateStateCondition();

        stateMachine.Update();

        SmoothDir = Vector2.SmoothDamp(
            SmoothDir,
            InputDir,
            ref smoothVelocity,
            SmoothTime
        );
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void OnDisable()
    {
        if (dm != null)
            dm.loadedDataGroup.Unsubscribe(LoadPlayerData);

        InputActionsDelete();
    }

    private void Init()
    {
        pm = PlayerManager.Instance;
        dm = DataManager.Instance;
        pm.instancePlayer = this;
        Status.Init();

        View = GetComponent<PlayerView>();
        Cc = GetComponent<ColliderController>();

        InputActionsInit();
        StateMachineInit();

        // ������ �ε��� �� Status�� �ε��� �����ͷ� ��ü
        dm.loadedDataGroup.Subscribe(LoadPlayerData);
    }

    private void LoadPlayerData(SaveDataGroup saveDataGroup)
    {
        // �÷��̾� ������ ����ȭ
        Status = saveDataGroup.playerStatusData;

        Status.Init_Load();

        // �κ��丮 Model ����ȭ
        Status.inventory.model = saveDataGroup.inventoryModel;

        // Model ���� ���� ����Ʈ(5��) ������ SlotData �� ������(SO)�� Key�����͸� Item���� �纯ȯ �� ��ġ��Ű��
        Status.inventory.model.LoadSlotData(saveDataGroup);

        // ��ġ �Ϸ� �� �� ������Ʈ
        Status.inventory.SetView(UIManager.Instance.inventoryGroup.inventoryView);
        Status.inventory.UpdateUI();

        Debug.Log("�÷��̾� ������ ������ �Լ� �Ϸ�");
    }

    public void StateMachineInit()
    {
        stateMachine.stateDic.Add(PlayerStateTypes.Idle, new Player_Idle(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Move, new Player_Move(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Sprint, new Player_Sprint(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Jump, new Player_Jump(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Fall, new Player_Fall(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Crouch, new Player_Crouch(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Attack, new Player_Attack(this));
        //
        stateMachine.stateDic.Add(PlayerStateTypes.Damaged, new Player_Damaged(this));
        stateMachine.stateDic.Add(PlayerStateTypes.Dead, new Player_Dead(this));

        stateMachine.CurState = stateMachine.stateDic[PlayerStateTypes.Idle];
    }

    private void InputActionsInit()
    {
        // ���� �Ұ��� ���·� ���� �� �׼ǵ��� Enable ���¸� ���� ���ִ� ������ ��������

        // �÷��̾� ���� ��
        var playerControlMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        // 1. ���� ī�޶�
        freeCamAction = playerControlMap.FindAction("FreeCamMod");
        freeCamAction.Enable();
        freeCamAction.performed += HandleFreeCam;
        freeCamAction.canceled += HandleFreeCam;

        // 2. ��ȣ �ۿ�
        interactAction = playerControlMap.FindAction("Interaction");
        interactAction.Enable();
        interactAction.performed += HandleInteract;

        /*// ���� (��Ŀ��)
        AimingAction = playerControlMap.FindAction("Aiming");
        AimingAction.Enable();
        AimingAction.performed += HandleAiming;
        AimingAction.canceled += HandleAiming;*/


        // 3. �޸��� �׼�
        sprintAction = playerControlMap.FindAction("Sprint");
        sprintAction.Enable();
        sprintAction.performed += HandleSprint;
        sprintAction.canceled += HandleSprint;

        // 4. ���� �׼�
        jumpAction = playerControlMap.FindAction("Jump");
        jumpAction.Enable();
        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJump;


        // 5. �ɱ� �׼�
        crouchAction = playerControlMap.FindAction("Crouch");
        crouchAction.Enable();
        crouchAction.performed += HandleCrouch;
        crouchAction.canceled += HandleCrouch;

        // 6. ���� �׼�
        attackAction = playerControlMap.FindAction("Attack");
        attackAction.Enable();
        attackAction.performed += HandleAttack;
        attackAction.canceled += HandleAttack;

        // 7. �κ��丮 ����(I)
        inventoryOpenAction = playerControlMap.FindAction("Inventory");
        inventoryOpenAction.Enable();
        inventoryOpenAction.performed += OpenInventory;

        // 8. ������ ��Ű
        quickSlotActions = playerControlMap.FindAction("QuickSlots");
        quickSlotActions.Enable();
        quickSlotActions.performed += OnQuickSlotPerformed;

        // 9. ESC Ű ó��
        escActions = playerControlMap.FindAction("ESC");
        escActions.Enable();
        escActions.performed += HandleESCKeyOnGameRuning;
    }

    private void InputActionsDelete()
    {
        // 1. ���� ī�޶�
        freeCamAction.performed -= HandleFreeCam;
        freeCamAction.canceled -= HandleFreeCam;

        // 2. ��ȣ�ۿ�
        interactAction.performed -= HandleInteract;

        /*AimingAction.performed -= HandleAiming;
        AimingAction.canceled -= HandleAiming;*/

        // 3. �޸��� �׼�
        sprintAction.performed -= HandleSprint;
        sprintAction.canceled -= HandleSprint;


        // 4. ���� �׼�
        jumpAction.performed -= HandleJump;
        jumpAction.canceled -= HandleJump;

        // 5. �ɱ� �׼�
        crouchAction.performed -= HandleCrouch;
        crouchAction.canceled -= HandleCrouch;

        // 6. ���� �׼�
        attackAction.performed -= HandleAttack;
        attackAction.canceled -= HandleAttack;

        // 7. �κ��丮 ����(I)
        attackAction.performed -= OpenInventory;

        // 8. ������ ��Ű
        quickSlotActions.performed -= OnQuickSlotPerformed;

        // 9. ESC Ű ó��
        escActions.performed -= HandleESCKeyOnGameRuning;

    }

    #region InputAction ó��
    public void OnMove(InputValue value)
    {
        InputDir = value.Get<Vector2>();
    }

    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseInputDir = mouseDir * Status.MouseSensitivity;
    }
    public void OnZoomInOut(InputValue value)
    {
        Vector2 scroll = value.Get<Vector2>();
        ZoomSet(scroll.y);
    }
    public void ZoomSet(float y)
    {
        if (y < 0)
        {
            if (currentZoomIndex < TPS_Cameras.Length - 1)
            {
                // ���� ī�޶� ����
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(false);

                // ���� �ܰ� ī�޶� �ѱ�
                currentZoomIndex++;
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(true);
            }
        }
        else if (y > 0)
        {
            if (currentZoomIndex > 0)
            {
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(false);

                currentZoomIndex--;
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(true);
            }
        }
    }

    public void HandleFreeCam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFreeCamModInput = true;
            View.FreeCamSet(true);
        }

        else if (context.canceled)
            isFreeCamModInput = false;
    }

    /*public void HandleAiming(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAimingInput = true;
            View.animator.SetBool("IsAiming", true);
        }
        else if (context.canceled)
        {
            isAimingInput = false;
            View.animator.SetBool("IsAiming", false);
        }
    }*/

    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            isSprintInput = true;
        else if (context.canceled)
            isSprintInput = false;
    }
    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            isJumpInput = true;
        if (context.canceled)
            isJumpInput = false;
    }

    public void HandleCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isCrouchToggle)
                isCrouchToggle = true;
            else
            {
                if (!Cc.GetIsHeadTouchedState())
                {
                    isCrouchToggle = false;
                }
            }
        }

    }

    public void HandleAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            isAttackInput = true;
        if (context.canceled)
            isAttackInput = false;
    }

    public void HandleInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            Interact();
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
            UIManager.Instance.inventoryGroup.inventoryView.TryOpenInventory();
    }

    public void HandleESCKeyOnGameRuning(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (UIManager.Instance.GetActivedPanelStack().Count > 0)
            {
                UIManager.Instance.ClosePanel();
            }
            // �г��� �� ���� ���¿��� esc�� ������
            else
            {
                Debug.Log("�Ͻ����� �� �ɼ��г� Ȱ��ȭ");
            }
        }

    }

    private void OnQuickSlotPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // ������ Ű �� �˻� (Keyboard.current.digit1Key.wasPressedThisFrame ��)
            if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectQuickSlot(1);
            if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectQuickSlot(2);
            if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectQuickSlot(3);
            if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectQuickSlot(4);
        }
    }
    private void SelectQuickSlot(int index)
    {
        Debug.Log($"Selected quick slot {index}");
        UIManager.Instance.inventoryGroup.quickSlotParent.SelectQuickSlot(index - 1);
        // �տ� ��� ������ ��ü
    }
    #endregion

    #region InputFlag�鿡 ���� ���� ��ȯ ���� ����
    public void UpdateStateCondition()
    {
        // ���� ���¿��� ó�� ����
        if (IsCurrentState(PlayerStateTypes.Dead)) return;

        // �������� ������
        if (IsCurrentState(PlayerStateTypes.Damaged))
        {
            stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Damaged]); return;
        }


        // �ٴ� üũ�� �������� Fall���·� ��ȯ
        if (!Cc.GetIsGroundState())
        {
            stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Fall]); return;
        }

        // ���� ���� ���¶��
        if (IsCurrentState(PlayerStateTypes.Jump) || IsCurrentState(PlayerStateTypes.Fall))
        {
            // ���� �������� �� (Idle�� �ȵ��ư�)
            if (IsCurrentState(PlayerStateTypes.Jump)) return;

            // �ٴ� üũ�Ǹ� �Ϲ� ����or�̵� ���� ��ȯ

            if (Cc.GetIsGroundState())
            {
                if (isSprintInput && isMoveInput) { stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Sprint]); return; }
                else if (isMoveInput) { stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Move]); return; }
                else { stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Idle]); return; }
            }

        }

        // �ٴ� ���¶��
        else
        {
            // => Attack ���� : 1. �Է°� ���� 
            if (isAttackInput)
            {
                // 2. �Ϲ� & �⺻ �̵� ������ �� ����
                if ((IsCurrentState(PlayerStateTypes.Idle) || IsCurrentState(PlayerStateTypes.Move)))
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Attack]);
                // 3. ���� ���� & �Ӹ����� ��ֹ��� ���� ���� ������ �� ����
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                        stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Attack]);
                }
                else
                {
                    return;
                }
            }


            // => Jump ���� : 1. �Է°� ����
            else if (isJumpInput)
            {
                // 2. ���� ���¿��� ���� ���� ����
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                // 3. ���� ���¶�� �Ӹ����� ���� ����ߵ�.
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                    {
                        stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Jump]);
                        isSprintJump = false;
                    }
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Jump]);
                }
            }

            // => Sprint ���� : 1. �Է°� ����
            else if (isSprintInput)
            {
                // 2. ���ݻ��¸� �޸��� ���� ��ȯ �ȵ�
                if (IsCurrentState(PlayerStateTypes.Attack)) return;

                // 3. ���� ���¶�� �Ӹ����� ���� ����ߵ�.
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                        stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Sprint]);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Sprint]);
                }
            }
            // => Crouch
            else if (isCrouchToggle)
            {
                // 2. ���ݻ��¸� �ɱ� ���� ��ȯ �ȵ�
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                else stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Crouch]);
            }
            // => Move
            else if (isMoveInput)
            {
                // �̵��� ��� �ִϸ��̼� setbool�� ��Ʈ�ѷ����� ����
                // 2. ���ݻ��¸� �̵� ���� ��ȯ �ȵ�
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Move]);
            }
            // => Idle
            else
            {
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Idle]);
            }
        }

    }

    #endregion

    #region �÷��̾� ���� ��� & ��ȣ�ۿ� ����

    public void HandleMove()
    {
        // ��Ʈ�� �� �ɸ��� �̵� ���� ����
        if (Status.isControllLocked) return;
        if (CameraManager.Instance.cinemachineBrain.IsBlending) return;


        float moveSpeed;
        if (IsCurrentState(PlayerStateTypes.Crouch)) moveSpeed = pm.CrouchSpeed;
        else if ( isSprinting /*IsCurrentState(PlayerStateTypes.Sprint) || 
            ((IsCurrentState(PlayerStateTypes.Jump) || IsCurrentState(PlayerStateTypes.Fall)) && isSprinting)*/ ) 
            moveSpeed = Status.SprintSpeed;
        else moveSpeed = Status.MoveSpeed;

        Vector3 getMoveDir;

        // ���̵� �� ����
        if (CameraManager.Instance.activeSideView)
        {
            SideView_Camera sideViewCam = CameraManager.Instance.sideViewCamera;
            getMoveDir = View.GetMoveDir_SideCamMode(InputDir, sideViewCam.front, sideViewCam.right);
        }
        else if (isFreeCamModInput)
            getMoveDir = View.GetMoveDirection(InputDir, true);
        else
            getMoveDir = View.GetMoveDirection(InputDir);


        Vector3 moveVec = View.SetMove(getMoveDir, moveSpeed);
        View.moveDir = moveVec;
        View.facingDir = moveVec;

        if (InputDir != Vector3.zero)
        {
            View.animator.SetBool("IsMove", true);
            isMoveInput = true;
        }
        else
        {
            View.animator.SetBool("IsMove", false);
            isMoveInput = false;
        }
    }

    public void HandleSight()
    {
        // ���̵� ķ Ȱ��ȭ ���¿��� ȭ��ȸ���� ����
        if (CameraManager.Instance.activeSideView)
        {
            View.SetAvatarRotation(View.facingDir, pm.RotateSpeed);
            return;
        }

        Vector3 camRotateDir = View.SetAimRotation(MouseInputDir, pm.MinPitch, pm.MaxPitch);

        Vector3 avatarDir;

        // ����ķ ��� => �÷��̾��� �̵� �������� �ƹ�Ÿ�� ���� �����ֱ�
        if (isFreeCamModInput) avatarDir = View.facingDir;
        // �� �ڸ��� ���缭�� ����ķ ��尡 �ƴ϶��, ���� �����̶�� =>  �ƹ�Ÿ�� �÷��̾��� ȭ���� ���� ����
        else if (!isMoveInput || IsCurrentState(PlayerStateTypes.Attack)) avatarDir = camRotateDir;
        else avatarDir = View.moveDir;



        // ��Ʈ�� �� �ɸ��� �ƹ�Ÿ ȸ���� ����
        if (Status.isControllLocked) return;
        View.SetAvatarRotation(avatarDir, pm.RotateSpeed);



        // Attack ������ ����.
        if (stateMachine.CurState == stateMachine.stateDic[PlayerStateTypes.Attack])
        {
            View.animator.SetFloat("MoveX", SmoothDir.x);
            View.animator.SetFloat("MoveZ", SmoothDir.y);
        }
    }

    public void CrouchToggleChange(bool value)
    {
        isCrouchToggle = value;
    }

    public void Attack()
    {
        IDamagable[] damagables = Cc.GetDamagablesInRange();

        if (damagables.Length < 1) return;

        float finalDamage = Status.Damage;
        if (Status.onHandItem is Item_Weapon weapon)
        {
            finalDamage += weapon.Damage;

        }

        foreach (IDamagable damagable in damagables)
        {
            damagable.TakeDamage(finalDamage, transform);
        }
    }

    public void Interact()
    {
        IInteractable interactable = Cc.InteractableObj;

        if (interactable != null)
            interactable.Interact();
    }

    public void KnockBack(Transform transform, float force)
    {
        // �÷��̾�� �������� ���� ���͸� ���(dir)     ##����: ���� ������ Y���� ���� ������ ���� �������� ����
        Vector3 basicDir = this.transform.position - transform.position;
        Vector3 basicDirToVector2 = new Vector3(basicDir.x, 0, basicDir.z);

        // ���� ���� + ���� ��¦ ��ģ ���͸� �������� ��
        Vector3 finalKnockBackDir = (basicDirToVector2.normalized + Vector3.up * 0.3f).normalized;

        GetComponent<Rigidbody>().AddForce(finalKnockBackDir * force, ForceMode.Impulse);

    }

    public void TakeDamage(float damage, Transform transform)
    {
        // ���� ���¶�� return;
        if (Status.isInvincible) return;
        // �̹� �ǰ� ���¶�� X
        if (IsCurrentState(PlayerStateTypes.Damaged)) return;

        // ���� ���¶�� ����X
        if (IsCurrentState(PlayerStateTypes.Dead)) return;

        // Ȱ�� ������ ��ü ���� �� ���� ����
        List<BodyPart> bodyPart = Status.GetBodyPartsList();
        List<BodyPart> activeBodyPart = new List<BodyPart>();

        foreach (BodyPart part in bodyPart)
        {
            // Ȱ�� ������ ������� ����Ʈ ���� ����
            if (part.Activate.Value)
                activeBodyPart.Add(part);
        }

        if (activeBodyPart.Count > 1)
        {
            // ���� ���� ������
            int r = Random.Range(1, activeBodyPart.Count);
            activeBodyPart[r].TakeDamage(damage);
        }
        else if (activeBodyPart.Count > 0)
        {
            // �Ӹ��� ���� ���¸� �Ӹ��� ������
            activeBodyPart[0].TakeDamage(damage);
        }

        KnockBack(transform, pm.knockBackForce_Init);

        Status.CheckCriticalState();
        stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Damaged]);
        StartCoroutine(InvincibleRoutine(pm.DamagedInvincibleTime));
    }

    public IEnumerator InvincibleRoutine(float time)
    {
        Status.isInvincible = true;

        // TODO : �÷��̾� �ǰ� ����Ʈ or ���̴� ����

        yield return new WaitForSeconds(time);

        Status.isInvincible = false;

        // TODO : �÷��̾� �ǰ� ����Ʈ or ���̴� �ʱ�ȭ
    }


    // �ܼ� ��ġ�� �̵����ֱ�
    public void Respawn(Transform transform)
    {
        if (transform == null) { Debug.LogError("�Ű����� Transform�� null��"); return; }
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }
    #endregion


    public void UpdateHandItem(Item item)
    {
        if (item == null)
        {
            if (onHandInstance != null) Destroy(onHandInstance);

            Status.onHandItem = null;
            // ���� ���� ȿ��
            View.animator.SetBool("Equip_Swing", false);
            View.animator.SetBool("Equip_Thrust", false);
        }
        else
        {
            if (onHandInstance != item.instancePrefab) Destroy(onHandInstance);

            onHandInstance = Instantiate(item.instancePrefab, handTransform);
            onHandInstance.GetComponent<Rigidbody>().isKinematic = true;
            Status.onHandItem = item;


            // ������ ���� ȿ��
            if (item is Item_Weapon weapon)
            {
                if (weapon.attackType == WeaponAttackType.Swing)
                {
                    View.animator.SetBool("Equip_Swing", true);
                    View.animator.SetBool("Equip_Thrust", false);

                }
                else if (weapon.attackType == WeaponAttackType.Thrust)
                {
                    View.animator.SetBool("Equip_Thrust", true);
                    View.animator.SetBool("Equip_Swing", false);
                }
            }
        }
    }


}
