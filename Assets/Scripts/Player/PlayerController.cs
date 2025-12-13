using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [field: SerializeField] public PlayerModel Model { get; private set; }
    [field: SerializeField] public PlayerView View { get; private set; }
    public ColliderController cc { get; private set; }
    public Rigidbody rb { get; private set; }

    // 상태 머신
    private PlayerStateMachine _fsm;
    public Dictionary<PlayerStateType, PlayerState> StateDic;

    // 입력 처리 (InputSystem 처리)
    [SerializeField] PlayerInputReader _inputReader;
    public PlayerInputData Input => _inputReader.Data;
    
    [SerializeField] Hitbox _defaultHitbox;
    public Hitbox CurrentHitbox // 무기 장착 상태에 따른 히트박스 반환
    {
        get
        {
            if (Model.Combat.HandedItem is Item_Weapon weapon && weapon.Hitbox != null)
                return weapon.Hitbox;
            else
                return _defaultHitbox;
        }
    }

    public Transform handTransform;
    [HideInInspector] public GameObject onHandInstance;


    private void Awake() => Init();

    private void Update()
    {
        if (!Model.IsControllLocked)
        {
            _fsm.HandleInput();
            HandleInGameInput();
        }
        _fsm.UpdateLogic();
        HandleUiInput();
    }

    private void LateUpdate()
    {
        // 일시 정지 상태가 아니라면
        Model.Tick(1f); // 초당 생존 수치 소모 루틴 실행

        // 이번 프레임에 사용한 Pressed/Released 플래그들 초기화
        _inputReader.BeginFrame();
    }

    private void FixedUpdate()
    {
        _fsm.FixedUpdateLogic();
        cc.GroundCheck();
        cc.HeadCheck();
    }

    private void OnDisable()
    {
        if (Manager.data != null)
            Manager.data.loadedDataGroup.Unsubscribe(LoadPlayerData);
    }

    private void Init()
    {
        rb = GetComponent<Rigidbody>();

        // Status & Model & View 초기화
        Manager.player.instancePlayer = this;
        Model = new();
        Model.Init();
        View ??= GetComponentInChildren<PlayerView>();
        cc ??= GetComponent<ColliderController>();

        // 상태머신 초기화
        _fsm = new PlayerStateMachine();
        StateDic = new();
        InitStateDictionary();
        var startState = StateDic[PlayerStateType.Idle];
        _fsm.Initialize(startState);

        // 데이터 로드할 때 Status를 로드한 데이터로 교체
        Manager.data.loadedDataGroup.Subscribe(LoadPlayerData);

        // bodyParts의 Model <-> View 연결
        Bind();

        _defaultHitbox.Init(transform);
    }

    public void InitStateDictionary()
    {
        StateDic.Add(PlayerStateType.Idle, new PlayerIdleState(this, _fsm));
        StateDic.Add(PlayerStateType.Move, new PlayerMoveState(this, _fsm));
        StateDic.Add(PlayerStateType.Roll, new PlayerRollState(this, _fsm));
        StateDic.Add(PlayerStateType.Jump, new PlayerJumpState(this, _fsm));
        StateDic.Add(PlayerStateType.Fall, new PlayerFallState(this, _fsm));
        StateDic.Add(PlayerStateType.Attack, new PlayerAttackState(this, _fsm));
        StateDic.Add(PlayerStateType.Hit, new PlayerHitState(this, _fsm));
    }

    public PlayerState GetState(PlayerStateType stateType)
    {
        return StateDic[stateType];
    }

    public void Bind()
    {
        Panel_PlayerStatus playerStatusUI = Manager.ui.inventoryGroup.panel_PlayerStatus;

        // 신체 부위 데이터 구독
        foreach (var kvp in Model.Survival.GetBodyPartsDic())
        {
            var bodyPart = kvp.Value;
            Panel_PartState partStateUI = playerStatusUI.dic_PartStatePanels[bodyPart.type];

            if (partStateUI != null)
            {
                // UI연동
                partStateUI.initMaxHp = bodyPart.InitMaxHp;
                partStateUI.UpdateHP_View(bodyPart.Hp.Value);
                partStateUI.UpdateCurrentMaxHP_View(bodyPart.CurrentMaxHp.Value);

                // UI 이벤트 구독
                bodyPart.Hp.Subscribe(partStateUI.UpdateHP_View);
                bodyPart.CurrentMaxHp.Subscribe(partStateUI.UpdateCurrentMaxHP_View);

                // 부위마다 체력 변화에 전체 부위 체력을 합산 계산하는 함수 구독
                bodyPart.Hp.Subscribe(Model.Survival.CalculateCurrentHPSum);
                bodyPart.CurrentMaxHp.Subscribe(Model.Survival.CalculateCurrentMaxHPSum);

                // 1회 초기화
                bodyPart.Init();
            }
        }

        // 체력 합산 수치 UI구독
        Model.Survival.SumCurrentHP.Subscribe(playerStatusUI.state_HpSum.UpdateStateNumb_View);
        Model.Survival.SumCurrentMaxHP.Subscribe(playerStatusUI.state_HpSum.UpdateMaxStateNumb_View);


        // 배터리 수치 UI 구독
        Model.Survival.CurrentBattery.Subscribe(playerStatusUI.state_Battery.UpdateStateNumb_View);
        Model.Survival.MaxBattery.Subscribe(playerStatusUI.state_Battery.UpdateMaxStateNumb_View);

        // 정신력 수치 UI 구독
        Model.Survival.CurrentWillPower.Subscribe(playerStatusUI.state_WillPower.UpdateStateNumb_View);

        // 배터리, 정신력 1회 초기화
        var initBatteryMax = Manager.data.CapacityTable.FindByKey("Battery").Max;
        var initWillMax = Manager.data.CapacityTable.FindByKey("Will").Max;

        playerStatusUI.state_Battery.initMax = initBatteryMax;
        playerStatusUI.state_WillPower.initMax = initWillMax;
        playerStatusUI.state_Battery.UpdateMaxStateNumb_View(initBatteryMax);
        playerStatusUI.state_WillPower.UpdateMaxStateNumb_View(initWillMax);
    }

    private void LoadPlayerData(SaveDataGroup saveDataGroup)
    {
        // 플레이어 데이터 동기화
        Model = saveDataGroup.playerStatusData;

        Model.Init_Load();

        // 인벤토리 Model 동기화
        Model.inventory.model = saveDataGroup.inventoryModel;

        // Model 내부 슬롯 리스트(5종) 내부의 SlotData 안 아이템(SO)의 Key데이터를 Item으로 재변환 후 배치시키기
        Model.inventory.model.LoadSlotData(saveDataGroup);

        // 배치 완료 후 뷰 업데이트
        //Status.inventory.SetView(UIManager.Instance.inventoryGroup.inventoryView);
        Model.inventory.UpdateUI();
    }

    public void HandleInGameInput()
    {
        HandleInteract(); // 이것도 아마 상태로 넘어갈듯? 애니메이션과 동작 제한이 있으니..
    }
    public void HandleUiInput()
    {
        HandleInventory();
        HandleQuickSlot();
        HandleEsc();
    }
    private void HandleInteract()
    {
        if (Input.InteractionPressed)
        {
            IInteractable interactable = cc.InteractableObj;

            if (interactable != null)
                interactable.Interact();
        }
    }

    private void HandleInventory()
    {
        if (Input.InventoryPressed)
        {
            Manager.ui.inventoryGroup.inventoryView.TryOpenInventory();
        }
    }

    private void HandleQuickSlot()
    {
        if (Input.QuickSlotIndexPressed != -1)
            Manager.ui.inventoryGroup.quickSlotParent.SelectQuickSlot(Input.QuickSlotIndexPressed);
    }

    private void HandleEsc()
    {
        if (Input.EscPressed)
        {
            var ui = Manager.ui;
            if (ui.GetActivedPanelStack().Count > 0)
            {
                ui.ClosePanel();
            }
            else
            {
                Debug.Log("열린 패널 없는 상태에서 Esc 누름 / 일시정지 옵션 패널 열기");
            }
        }
    }


    // 애니메이션 보조 스크립트
    public void OnLandAnimationFinished()
    {
        Model.IsLanding = false;
    }

    public void OnAttackAnimationStarted()
    {
        CurrentHitbox.SetActive(true);
    }

    public void OnAttackAnimationFinished()
    {
        CurrentHitbox.SetActive(false);

        if (_fsm.CurState is PlayerAttackState attack)  // 공격 상태에서 이미 벗어난 경우 그대로 놔두기 (ex. 공격 도중 낙하 or 피격)
            attack.QuitAttack();
    }

    // 피격 받음
    public void TakeDamage(HitInfo hit)
    {
        // 무적 상태라면 return;
        if (Model.IsInvincible) return;

        // Model에서 데미지 계산 & 죽음 판단
        Model.TakeDamage(hit.Damage);

        if (Model.IsDead) // 죽으면 넉백 & 피격 애니 실행 x
        {
            // 죽음 애니 / 상태 전환 / SFX & VFX 실행
            // _fsm.ChangeState(StateDic[PlayerStateType.Dead]);
            return;
        } 
        else
        {
            // View에서 넉백 & 피격 애니메이션 + SFX & VFX 실행

            // HitInfo 캐싱 및 Hit 상태 전환
            var hitState = StateDic[PlayerStateType.Hit] as PlayerHitState;
            hitState.CashingHitInfo(hit);
            _fsm.ChangeState(hitState);
        }
    }

    // 단순 위치만 이동해주기
    public void Respawn(Transform transform)
    {
        if (transform == null) { Debug.LogError("매개변수 Transform이 null임"); return; }
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }

    public void UpdateHandItem(Item item)
    {
        if (item == null)
        {
            if (onHandInstance != null) Destroy(onHandInstance);

            Model.Combat.HandedItem = null;
            
            // 장착 해제 효과
        }
        else
        {
            if (onHandInstance != item.instancePrefab) Destroy(onHandInstance);

            onHandInstance = Instantiate(item.instancePrefab, handTransform);
            Debug.Log("소환");
            onHandInstance.GetComponent<ItemInstance>().isUsed = true;
            onHandInstance.GetComponent<Rigidbody>().isKinematic = true;
            Model.Combat.HandedItem = item;

            // 무기라면 히트박스 설정
            if (Model.Combat.HandedItem is Item_Weapon weapon)
            {
                weapon.Hitbox = onHandInstance.GetComponentInChildren<Hitbox>();
                weapon.Hitbox.Init(transform);
            }
        }
    }


}
