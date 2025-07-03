using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    //[field: SerializeField] public float AttackCoolTime { get; private set; }
    public bool isAttacking;

    // [���̺� & �ε� ������]
    // Transform << ���� ����ҵ� ������ּ���

    // [���̺� & �ε� ������]
    private PlayerManager pm;

    [field: SerializeField] public PlayerStatus Status { get; private set; }

    public PlayerView View { get; private set; }
    public ColliderController Cc { get; private set; }

    public Vector3 InputDir { get; private set; }
    public Vector2 MouseInputDir { get; private set; }

    [SerializeField] Transform[] TPS_Cameras;
    int currentZoomIndex = 1;

    private InputAction AimingAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction attackAction;
    private InputAction freeCamAction;
    private InputAction interactAction;
    private InputAction inventoryOpenAction;
    private InputAction inventoryOffAction;
    private InputAction quickSlotActions;


    //Vector3 moveDir;

    bool isMoveInput;
    bool isSprintInput;
    bool isJumpInput;
    bool isCrouchToggle;
    bool isAimingInput;
    bool isFreeCamModInput;
    bool isAttackInput;


    public StateMachine<PlayerStateTypes> stateMachine = new StateMachine<PlayerStateTypes>();

    public bool IsCurrentState(PlayerStateTypes state)
    {
        return stateMachine.CurState == stateMachine.stateDic[state];
    }

    private void Awake() => Init();

    public Vector2 SmoothDir;      // ĳ���Ͱ� ������ �� ����
    public float SmoothTime = 0.1f; // ���� �ӵ�
    private Vector2 smoothVelocity; // SmoothDamp�� ���� ����
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

    private void OnDestroy() => InputActionsDelete();

    private void Init()
    {
        pm = PlayerManager.Instance;
        pm.instancePlayer = this;

        Status.Init();
        View = GetComponent<PlayerView>();
        Cc = GetComponent<ColliderController>();

        InputActionsInit();
        StateMachineInit();


        // ������ �ε��� �� Status�� �ε��� �����ͷ� ��ü
        DataManager dm = DataManager.Instance;
        dm.loadedDataGroup.Subscribe(LoadPlayerData);
    }

    private void LoadPlayerData(SaveDataGroup saveDataGroup)
    {
        // �÷��̾� ������ ����ȭ
        Status = saveDataGroup.playerStatusData;

        // �κ��丮 Model ����ȭ
        Status.inventory.model = saveDataGroup.inventoryModel;

        // Model ���� ���� ����Ʈ(5��) ������ SlotData �� ������(SO)�� Key�����͸� Item���� �纯ȯ �� ��ġ��Ű��
        Status.inventory.model.LoadSlotData(saveDataGroup);
        Status.inventory.view.CurrentTab.UnsbscribeAll();

        // ��ġ �Ϸ� �� �� ������Ʈ
        Status.inventory.SetView(UIManager.Instance.inventoryGroup.inventoryView);
        Status.inventory.UpdateUI();
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
        // ��Ʈ�� �� �ɸ��� 
        if (Status.isControllLocked)
        {
            //View.animator
            return;
        }

        // �ٴ� ���¶��
        if (Cc.GetIsGroundState())
        {
            // => Attack ���� : �Է°� ���� && �Ϲ� or �⺻�̵� ������ ���� ����
            if (isAttackInput || isAttacking)
            {
                if ((IsCurrentState(PlayerStateTypes.Idle) || IsCurrentState(PlayerStateTypes.Move)))
                {
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Attack]);
                }
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
            // => Jump
            else if (isJumpInput)
            {
                if (isAttacking)
                {
                    return;
                }
                else if (IsCurrentState(PlayerStateTypes.Crouch))
                {
                    if (!Cc.GetIsHeadTouchedState())
                        stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Jump]);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Jump]);
                }
            }
            // => Sprint
            else if (isSprintInput)
            {
                if (isAttacking)
                {
                    return;
                }
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
                if (isAttacking) return;
                else stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Crouch]);
            }
            // => Move
            else if (isMoveInput)
            {
                stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Move]);
            }
            // => Idle
            else
            {

                stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Idle]);
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Fall]);
        }
    }

    #endregion

    #region �÷��̾� ���� ��� & ��ȣ�ۿ� ����

    public void HandleMove()
    {
        // ��Ʈ�� �� �ɸ��� �̵� ���� ����
        if (Status.isControllLocked) return;

        float moveSpeed;
        if (IsCurrentState(PlayerStateTypes.Crouch)) moveSpeed = pm.CrouchSpeed;
        else if (isSprintInput && !isAttacking) moveSpeed = Status.SprintSpeed;
        else moveSpeed = Status.MoveSpeed;

        Vector3 getMoveDir;

        if (isFreeCamModInput)
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
        Debug.Log("���ý���");
        IDamagable[] damagables = Cc.GetDamagablesInRange();

        if (damagables.Length < 1) return;
        Debug.Log("���ý���01");

        int finalDamage = Status.Damage;
        if (Status.onHandItem is Item_Weapon weapon)
        {
            finalDamage += weapon.Damage;
        }
        Debug.Log("���ý���02");

        foreach (IDamagable damagable in damagables)
        {
            Debug.Log("���ý���03");

            damagable.TakeDamage(finalDamage, transform);

            Debug.Log("���ý���04");
        }
    }

    public void Interact()
    {
        IInteractable interactable = Cc.InteractableObj;

        if (interactable != null)
            interactable.Interact();
    }

    public void TakeDamage(int damage, Transform transform)
    {
        // ���� ���¶�� return;
        if (Status.isInvincible) return;

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

        Status.CheckCriticalState();
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
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }
    #endregion



}
