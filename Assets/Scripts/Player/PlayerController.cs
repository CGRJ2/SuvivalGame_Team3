using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
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


    // 손에 장착한 아이템 << Model로 이동하는게 맞는 듯
    public Transform handTransform;
    [HideInInspector] public GameObject onHandInstance;


    private void Awake() => Init();

    private void Update()
    {
        _fsm.HandleInput();
        _fsm.UpdateLogic();
    }

    private void LateUpdate()
    {
        // 이번 프레임에 사용한 Pressed/Released 플래그들 초기화
        _inputReader.BeginFrame();
    }

    private void FixedUpdate()
    {
        //HandleMove();
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
    }

    public void InitStateDictionary()
    {
        StateDic.Add(PlayerStateType.Idle, new PlayerIdleState(this, _fsm));
        StateDic.Add(PlayerStateType.Move, new PlayerMoveState(this, _fsm));
        StateDic.Add(PlayerStateType.Roll, new PlayerRollState(this, _fsm));
        StateDic.Add(PlayerStateType.Jump, new PlayerJumpState(this, _fsm));
        StateDic.Add(PlayerStateType.Fall, new PlayerFallState(this, _fsm));
        StateDic.Add(PlayerStateType.Attack, new PlayerAttackState(this, _fsm));
    }

    public PlayerState GetState(PlayerStateType stateType)
    {
        return StateDic[stateType];
    }

    public void Bind()
    {
        Panel_PlayerStatus playerStatusUI = Manager.ui.inventoryGroup.panel_PlayerStatus;

        // 신체 부위 데이터 구독
        foreach (var kvp in Model.GetBodyPartsDic())
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
                bodyPart.Hp.Subscribe(Model.CalculateCurrentHPSum);
                bodyPart.CurrentMaxHp.Subscribe(Model.CalculateCurrentMaxHPSum);

                // 1회 초기화
                bodyPart.Init();
            }
        }

        // 체력 합산 수치 UI구독
        Model.SumCurrentHP.Subscribe(playerStatusUI.state_HpSum.UpdateStateNumb_View);
        Model.SumCurrentMaxHP.Subscribe(playerStatusUI.state_HpSum.UpdateMaxStateNumb_View);


        // 배터리 수치 UI 구독
        Model.CurrentBattery.Subscribe(playerStatusUI.state_Battery.UpdateStateNumb_View);
        Model.MaxBattery.Subscribe(playerStatusUI.state_Battery.UpdateMaxStateNumb_View);

        // 정신력 수치 UI 구독
        Model.CurrentWillPower.Subscribe(playerStatusUI.state_WillPower.UpdateStateNumb_View);

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
   
    // Controller에서는 Attack의 시점만 판단. Attack의 결과는 View에서 주먹 콜라이더 펀치 또는 무기 콜라이더 휘두르기/찌르기 로 구분해서 실행
    public void TryAttack() 
    {
        Debug.Log("어택 실행됨");

        float finalDamage = Model.Damage;

        // View의 animator에서 공격 애니메이션을 실행
        // 애니메이션 이벤트에서 무기를 휘두르는 or 주먹을 뻗는 시점에서 공격 히트박스 활성화
        // 기본 히트박스 -> 주먹
        // 무기 히트박스 -> 무기 인스턴스 별로 프리펩에 설정해두기
    }

    public void TryInteract()
    {
        IInteractable interactable = cc.InteractableObj;

        if (interactable != null)
            interactable.Interact();
    }

    public void ApplyKnockBack(HitInfo hitInfo)
    {
        // 공격 방향 + 위로 살짝 합친 벡터를 방향으로 함
        Vector3 finalKnockBackDir = (hitInfo.KnockbackDir + Vector3.up * 0.3f).normalized;

        GetComponent<Rigidbody>().AddForce(finalKnockBackDir * hitInfo.KnockbackPower, ForceMode.Impulse);
    }

    public void TakeDamage(HitInfo hitInfo)
    {
        // 무적 상태라면 return;
        if (Model.isInvincible) return;
        // 이미 피격 상태라면 X
        //if (_fsm.CurState is PlayerHitState) return;

        // 죽음 상태라면 실행X
        //if (_fsm.CurState is PlayerDeadState) return;

        // Model에서 데미지 계산 & 죽음 판단
        Model.TakeDamage(hitInfo.Damage);

        // View에서 넉백 & 피격 애니메이션 + SFX & VFX 실행

        // 넉백 물리처리
        ApplyKnockBack(hitInfo);


        //
        //stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Damaged]);
        StartCoroutine(InvincibleRoutine(Manager.player.DamagedInvincibleTime));
    }

    public IEnumerator InvincibleRoutine(float time)
    {
        Model.isInvincible = true;

        // TODO : 플레이어 피격 이펙트 or 셰이더 실행

        yield return new WaitForSeconds(time);

        Model.isInvincible = false;

        // TODO : 플레이어 피격 이펙트 or 셰이더 초기화
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

            Model.onHandItem = null;
            // 장착 해제 효과
            View.Animator.SetBool("Equip_Swing", false);
            View.Animator.SetBool("Equip_Thrust", false);
        }
        else
        {
            if (onHandInstance != item.instancePrefab) Destroy(onHandInstance);

            onHandInstance = Instantiate(item.instancePrefab, handTransform);
            Debug.Log("소환");
            onHandInstance.GetComponent<ItemInstance>().isUsed = true;
            onHandInstance.GetComponent<Rigidbody>().isKinematic = true;
            Model.onHandItem = item;

            // 아이템 장착 효과
            if (item is Item_Weapon weapon)
            {
                if (weapon.attackType == WeaponAttackType.Swing)
                {
                    View.Animator.SetBool("Equip_Swing", true);
                    View.Animator.SetBool("Equip_Thrust", false);

                }
                else if (weapon.attackType == WeaponAttackType.Thrust)
                {
                    View.Animator.SetBool("Equip_Thrust", true);
                    View.Animator.SetBool("Equip_Swing", false);
                }
            }
            else if (item is Item_Throwing throwings)
            {

            }
        }
    }


}
