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

    private Vector2 SmoothDir;      // 캐릭터가 실제로 쓸 방향
    private float SmoothTime = 0.1f; // 보간 속도
    private Vector2 smoothVelocity; // SmoothDamp용 내부 변수

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

        // 데이터 로드할 때 Status를 로드한 데이터로 교체
        dm.loadedDataGroup.Subscribe(LoadPlayerData);
    }

    private void LoadPlayerData(SaveDataGroup saveDataGroup)
    {
        // 플레이어 데이터 동기화
        Status = saveDataGroup.playerStatusData;

        Status.Init_Load();

        // 인벤토리 Model 동기화
        Status.inventory.model = saveDataGroup.inventoryModel;

        // Model 내부 슬롯 리스트(5종) 내부의 SlotData 안 아이템(SO)의 Key데이터를 Item으로 재변환 후 배치시키기
        Status.inventory.model.LoadSlotData(saveDataGroup);

        // 배치 완료 후 뷰 업데이트
        Status.inventory.SetView(UIManager.Instance.inventoryGroup.inventoryView);
        Status.inventory.UpdateUI();

        Debug.Log("플레이어 데이터 구독자 함수 완료");
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

        // 9. ESC 키 처리
        escActions = playerControlMap.FindAction("ESC");
        escActions.Enable();
        escActions.performed += HandleESCKeyOnGameRuning;
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

        // 9. ESC 키 처리
        escActions.performed -= HandleESCKeyOnGameRuning;

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

    public void HandleESCKeyOnGameRuning(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (UIManager.Instance.GetActivedPanelStack().Count > 0)
            {
                UIManager.Instance.ClosePanel();
            }
            // 패널이 다 닫힌 상태에서 esc를 누르면
            else
            {
                Debug.Log("일시정지 및 옵션패널 활성화");
            }
        }

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
        // 죽음 상태에선 처리 안함
        if (IsCurrentState(PlayerStateTypes.Dead)) return;

        // 데미지를 받으면
        if (IsCurrentState(PlayerStateTypes.Damaged))
        {
            stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Damaged]); return;
        }


        // 바닥 체크가 없어지면 Fall상태로 전환
        if (!Cc.GetIsGroundState())
        {
            stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Fall]); return;
        }

        // 현재 공중 상태라면
        if (IsCurrentState(PlayerStateTypes.Jump) || IsCurrentState(PlayerStateTypes.Fall))
        {
            // 점프 진행중일 때 (Idle로 안돌아감)
            if (IsCurrentState(PlayerStateTypes.Jump)) return;

            // 바닥 체크되면 일반 상태or이동 상태 전환

            if (Cc.GetIsGroundState())
            {
                if (isSprintInput && isMoveInput) { stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Sprint]); return; }
                else if (isMoveInput) { stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Move]); return; }
                else { stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Idle]); return; }
            }

        }

        // 바닥 상태라면
        else
        {
            // => Attack 조건 : 1. 입력값 존재 
            if (isAttackInput)
            {
                // 2. 일반 & 기본 이동 상태일 때 가능
                if ((IsCurrentState(PlayerStateTypes.Idle) || IsCurrentState(PlayerStateTypes.Move)))
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Attack]);
                // 3. 앉음 상태 & 머리위에 장애물이 막지 않은 상태일 때 가능
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


            // => Jump 조건 : 1. 입력값 존재
            else if (isJumpInput)
            {
                // 2. 공격 상태에선 점프 실행 안함
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                // 3. 앉은 상태라면 머리위에 뭔가 없어야됨.
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

            // => Sprint 조건 : 1. 입력값 존재
            else if (isSprintInput)
            {
                // 2. 공격상태면 달리기 상태 전환 안됨
                if (IsCurrentState(PlayerStateTypes.Attack)) return;

                // 3. 앉은 상태라면 머리위에 뭔가 없어야됨.
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
                // 2. 공격상태면 앉기 상태 전환 안됨
                if (IsCurrentState(PlayerStateTypes.Attack)) return;
                else stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Crouch]);
            }
            // => Move
            else if (isMoveInput)
            {
                // 이동의 경우 애니메이션 setbool을 컨트롤러에서 하자
                // 2. 공격상태면 이동 상태 전환 안됨
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

    #region 플레이어 조작 기능 & 상호작용 관리

    public void HandleMove()
    {
        // 컨트롤 락 걸리면 이동 로직 중지
        if (Status.isControllLocked) return;
        if (CameraManager.Instance.cinemachineBrain.IsBlending) return;


        float moveSpeed;
        if (IsCurrentState(PlayerStateTypes.Crouch)) moveSpeed = pm.CrouchSpeed;
        else if ( isSprinting /*IsCurrentState(PlayerStateTypes.Sprint) || 
            ((IsCurrentState(PlayerStateTypes.Jump) || IsCurrentState(PlayerStateTypes.Fall)) && isSprinting)*/ ) 
            moveSpeed = Status.SprintSpeed;
        else moveSpeed = Status.MoveSpeed;

        Vector3 getMoveDir;

        // 사이드 뷰 들어가면
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
        // 사이드 캠 활성화 상태에선 화면회전은 정지
        if (CameraManager.Instance.activeSideView)
        {
            View.SetAvatarRotation(View.facingDir, pm.RotateSpeed);
            return;
        }

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
        // 플레이어와 공격자의 방향 벡터를 얻기(dir)     ##주의: 방향 벡터의 Y값을 빼서 평면상의 벡터 방향으로 설정
        Vector3 basicDir = this.transform.position - transform.position;
        Vector3 basicDirToVector2 = new Vector3(basicDir.x, 0, basicDir.z);

        // 공격 방향 + 위로 살짝 합친 벡터를 방향으로 함
        Vector3 finalKnockBackDir = (basicDirToVector2.normalized + Vector3.up * 0.3f).normalized;

        GetComponent<Rigidbody>().AddForce(finalKnockBackDir * force, ForceMode.Impulse);

    }

    public void TakeDamage(float damage, Transform transform)
    {
        // 무적 상태라면 return;
        if (Status.isInvincible) return;
        // 이미 피격 상태라면 X
        if (IsCurrentState(PlayerStateTypes.Damaged)) return;

        // 죽음 상태라면 실행X
        if (IsCurrentState(PlayerStateTypes.Dead)) return;

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

        KnockBack(transform, pm.knockBackForce_Init);

        Status.CheckCriticalState();
        stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Damaged]);
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
        if (transform == null) { Debug.LogError("매개변수 Transform이 null임"); return; }
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
            // 장착 해제 효과
            View.animator.SetBool("Equip_Swing", false);
            View.animator.SetBool("Equip_Thrust", false);
        }
        else
        {
            if (onHandInstance != item.instancePrefab) Destroy(onHandInstance);

            onHandInstance = Instantiate(item.instancePrefab, handTransform);
            onHandInstance.GetComponent<Rigidbody>().isKinematic = true;
            Status.onHandItem = item;


            // 아이템 장착 효과
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
