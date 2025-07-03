using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    //[field: SerializeField] public float AttackCoolTime { get; private set; }
    public bool isAttacking;

    // [세이브 & 로드 데이터]
    // Transform << 정보 저장소도 만들어주세요

    // [세이브 & 로드 데이터]
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

    public Vector2 SmoothDir;      // 캐릭터가 실제로 쓸 방향
    public float SmoothTime = 0.1f; // 보간 속도
    private Vector2 smoothVelocity; // SmoothDamp용 내부 변수
    private void Update()
    {
        ///////////////////////////
        /// 테스트
        /// 
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(10, transform);
        }
        ///
        /////////////////////////////
        HandleSight(); // 화면 회전은 isControllLocked로 부터 자유로움


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


        // 데이터 로드할 때 Status를 로드한 데이터로 교체
        DataManager dm = DataManager.Instance;
        dm.loadedDataGroup.Subscribe(LoadPlayerData);
    }

    private void LoadPlayerData(SaveDataGroup saveDataGroup)
    {
        // 플레이어 데이터 동기화
        Status = saveDataGroup.playerStatusData;

        // 인벤토리 Model 동기화
        Status.inventory.model = saveDataGroup.inventoryModel;

        // Model 내부 슬롯 리스트(5종) 내부의 SlotData 안 아이템(SO)의 Key데이터를 Item으로 재변환 후 배치시키기
        Status.inventory.model.LoadSlotData(saveDataGroup);
        Status.inventory.view.CurrentTab.UnsbscribeAll();

        // 배치 완료 후 뷰 업데이트
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
        // 조작 불가능 상태로 만들 때 액션들의 Enable 상태를 껐다 켜주는 식으로 진행하자

        // 플레이어 조작 맵
        var playerControlMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        // 1. 자유 카메라
        freeCamAction = playerControlMap.FindAction("FreeCamMod");
        freeCamAction.Enable();
        freeCamAction.performed += HandleFreeCam;
        freeCamAction.canceled += HandleFreeCam;

        // 2. 상호 작용
        interactAction = playerControlMap.FindAction("Interaction");
        interactAction.Enable();
        interactAction.performed += HandleInteract;

        /*// 조준 (포커싱)
        AimingAction = playerControlMap.FindAction("Aiming");
        AimingAction.Enable();
        AimingAction.performed += HandleAiming;
        AimingAction.canceled += HandleAiming;*/


        // 3. 달리기 액션
        sprintAction = playerControlMap.FindAction("Sprint");
        sprintAction.Enable();
        sprintAction.performed += HandleSprint;
        sprintAction.canceled += HandleSprint;

        // 4. 점프 액션
        jumpAction = playerControlMap.FindAction("Jump");
        jumpAction.Enable();
        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJump;


        // 5. 앉기 액션
        crouchAction = playerControlMap.FindAction("Crouch");
        crouchAction.Enable();
        crouchAction.performed += HandleCrouch;
        crouchAction.canceled += HandleCrouch;

        // 6. 공격 액션
        attackAction = playerControlMap.FindAction("Attack");
        attackAction.Enable();
        attackAction.performed += HandleAttack;
        attackAction.canceled += HandleAttack;

        // 7. 인벤토리 열기(I)
        inventoryOpenAction = playerControlMap.FindAction("Inventory");
        inventoryOpenAction.Enable();
        inventoryOpenAction.performed += OpenInventory;

        // 8. 퀵슬롯 핫키
        quickSlotActions = playerControlMap.FindAction("QuickSlots");
        quickSlotActions.Enable();
        quickSlotActions.performed += OnQuickSlotPerformed;
    }

    private void InputActionsDelete()
    {
        // 1. 자유 카메라
        freeCamAction.performed -= HandleFreeCam;
        freeCamAction.canceled -= HandleFreeCam;

        // 2. 상호작용
        interactAction.performed -= HandleInteract;

        /*AimingAction.performed -= HandleAiming;
        AimingAction.canceled -= HandleAiming;*/

        // 3. 달리기 액션
        sprintAction.performed -= HandleSprint;
        sprintAction.canceled -= HandleSprint;


        // 4. 점프 액션
        jumpAction.performed -= HandleJump;
        jumpAction.canceled -= HandleJump;

        // 5. 앉기 액션
        crouchAction.performed -= HandleCrouch;
        crouchAction.canceled -= HandleCrouch;

        // 6. 공격 액션
        attackAction.performed -= HandleAttack;
        attackAction.canceled -= HandleAttack;

        // 7. 인벤토리 열기(I)
        attackAction.performed -= OpenInventory;

        // 8. 퀵슬롯 핫키
        quickSlotActions.performed -= OnQuickSlotPerformed;

    }

    #region InputAction 처리
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
                // 현재 카메라 끄기
                TPS_Cameras[currentZoomIndex].gameObject.SetActive(false);

                // 다음 단계 카메라 켜기
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
            // 디지털 키 값 검사 (Keyboard.current.digit1Key.wasPressedThisFrame 등)
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
        // 손에 드는 아이템 교체
    }
    #endregion

    #region InputFlag들에 따른 상태 전환 조건 관리
    public void UpdateStateCondition()
    {
        // 컨트롤 락 걸리면 
        if (Status.isControllLocked)
        {
            //View.animator
            return;
        }

        // 바닥 상태라면
        if (Cc.GetIsGroundState())
        {
            // => Attack 조건 : 입력값 존재 && 일반 or 기본이동 상태일 때만 가능
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

    #region 플레이어 조작 기능 & 상호작용 관리

    public void HandleMove()
    {
        // 컨트롤 락 걸리면 이동 로직 중지
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
        // 프리캠 모드 => 플레이어의 이동 방향으로 아바타의 방향 맞춰주기
        if (isFreeCamModInput) avatarDir = View.facingDir;
        // 제 자리에 멈춰서서 프리캠 모드가 아니라면, 공격 도중이라면 =>  아바타가 플레이어의 화면을 향해 응시
        else if (!isMoveInput || IsCurrentState(PlayerStateTypes.Attack)) avatarDir = camRotateDir;
        else avatarDir = View.moveDir;

        // 컨트롤 락 걸리면 아바타 회전은 정지
        if (Status.isControllLocked) return;

        View.SetAvatarRotation(avatarDir, pm.RotateSpeed);

        // Attack 상태일 때만.
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
        Debug.Log("어택실행");
        IDamagable[] damagables = Cc.GetDamagablesInRange();

        if (damagables.Length < 1) return;
        Debug.Log("어택실행01");

        int finalDamage = Status.Damage;
        if (Status.onHandItem is Item_Weapon weapon)
        {
            finalDamage += weapon.Damage;
        }
        Debug.Log("어택실행02");

        foreach (IDamagable damagable in damagables)
        {
            Debug.Log("어택실행03");

            damagable.TakeDamage(finalDamage, transform);

            Debug.Log("어택실행04");
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
        // 무적 상태라면 return;
        if (Status.isInvincible) return;

        // 활성 상태인 신체 부위 중 랜덤 선택
        List<BodyPart> bodyPart = Status.GetBodyPartsList();
        List<BodyPart> activeBodyPart = new List<BodyPart>();

        foreach (BodyPart part in bodyPart)
        {
            // 활성 상태인 파츠들로 리스트 새로 생성
            if (part.Activate.Value)
                activeBodyPart.Add(part);
        }

        if (activeBodyPart.Count > 1)
        {
            // 부위 랜덤 데미지
            int r = Random.Range(1, activeBodyPart.Count);
            activeBodyPart[r].TakeDamage(damage);
        }
        else if (activeBodyPart.Count > 0)
        {
            // 머리만 남은 상태면 머리에 데미지
            activeBodyPart[0].TakeDamage(damage);
        }

        Status.CheckCriticalState();
        StartCoroutine(InvincibleRoutine(pm.DamagedInvincibleTime));
    }

    public IEnumerator InvincibleRoutine(float time)
    {
        Status.isInvincible = true;
        // TODO : 플레이어 피격 이펙트 or 셰이더 실행

        yield return new WaitForSeconds(time);

        Status.isInvincible = false;
        // TODO : 플레이어 피격 이펙트 or 셰이더 초기화
    }


    // 단순 위치만 이동해주기
    public void Respawn(Transform transform)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }
    #endregion



}
