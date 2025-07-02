using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : BaseMonster
{
    public enum CatDetectionTarget { None, CatBait, Player }
    public CatMonsterSO CatData => data as CatMonsterSO;
    public bool IsInvincible { get; private set; } = true;

    private List<Transform> baitTransforms = new List<Transform>();
    private Transform playerTransform;
    public Transform respawnPoint;

    protected override void Awake()
    {
        stateFactory = new CatMonsterStateFactory(this);
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        IsInvincible = true;
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        RefreshBaitList();
    }
    private void OnEnable()
    {
        DailyManager.Instance.TZ_State.Subscribe(OnTimeZoneChanged);
    }

    private void OnDisable()
    {
        DailyManager.Instance.TZ_State.Unsubscribe(OnTimeZoneChanged);
    }
    private void OnTimeZoneChanged(TimeZoneState newState)
    {
        Debug.Log("고양이 시간대 변경됨: " + newState);
        UpdateSightParameters();

        if (newState == TimeZoneState.Morning)
        {
            MoveToRespawn();
            StateMachine.ChangeState(new CatSleepState());
            Debug.Log("[Cat] 오전, 수면 상태 진입");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                //player.StartCatCutscene(this); // this = CatAI 인스턴스
                                               // (여기서는 아무 컷씬 정보 없음)
            }
        }
    }
    protected override void HandleState()
    {
        if (IsDead)
        {
            if (!(stateMachine.CurrentState is MonsterDeadState))
                stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        if (CheckTargetVisible())
        {
            var alertState = StateFactory.GetStateForPerception(MonsterPerceptionState.Alert);
            if (stateMachine.CurrentState != alertState)
                stateMachine.ChangeState(alertState);
        }
        else
        {
            var idleState = StateFactory.GetStateForPerception(MonsterPerceptionState.Idle);
            if (stateMachine.CurrentState != idleState)
                stateMachine.ChangeState(idleState);
        }
    }

    public void ApplyPacifyEffect(float duration)
    {
        // 외부 자극(아이템 등)으로 인해 무력화 상태 진입
        SetPerceptionState(MonsterPerceptionState.Idle);
        StateMachine.ChangeState(new CatPacifiedState(duration));
    }

    public bool IsPlayerMakingNoise()
    {
        var player = GetTarget()?.GetComponent<PlayerStatus>();
        if (player == null) return false;
        return !player.IsCurrentState(PlayerStateTypes.Crouch) && !player.IsCurrentState(PlayerStateTypes.Idle);
    }
    public bool IsPlayerInDetectionRange()
    {
        if (playerTransform == null) return false;
        return !GetComponent<PlayerStatus>()?.IsCurrentState(PlayerStateTypes.Idle) ?? false;
    }
    public bool IsInDetectionRange(Transform target)
    {
        if (target == null) return false;
        float range = CatData.catDetectionRange;
        float dist = Vector3.Distance(transform.position, target.position);
        return dist <= range;
    }
    public bool IsInFootDetectionRange(Transform target)
    {
        if (target == null) return false;
        float range = CatData.footstepDetectionRange;
        float dist = Vector3.Distance(transform.position, target.position);
        return dist <= range;
    }
    public CatDetectionTarget GetDetectionTarget()
    {
        // CatBait 감지
        foreach (var bait in baitTransforms)
        {
            if (bait == null) continue;
            if (IsInFootDetectionRange(bait))
                return CatDetectionTarget.CatBait;
        }
        // Player 감지
        if (IsPlayerMakingNoise() && IsInFootDetectionRange(playerTransform))
            return CatDetectionTarget.Player;

        return CatDetectionTarget.None;
    }
    public void RefreshBaitList()
    {
        baitTransforms.Clear();
        var baits = GameObject.FindGameObjectsWithTag("CatBait");
        foreach (var bait in baits)
            baitTransforms.Add(bait.transform);
    }

    public CatDetectionTarget GetClosestTarget(out Transform closest)
    {
        closest = null;
        float minDist = float.MaxValue;
        CatDetectionTarget found = CatDetectionTarget.None;

        // 1. CatBait 먼저 탐색
        foreach (var bait in baitTransforms)
        {
            if (bait == null) continue;
            float dist = Vector3.Distance(transform.position, bait.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = bait;
                found = CatDetectionTarget.CatBait;
            }
        }

        // 2. Player가 더 가까우면 덮어씀
        if (playerTransform != null)
        {
            float playerDist = Vector3.Distance(transform.position, playerTransform.position);
            if (playerDist < minDist)
            {
                minDist = playerDist;
                closest = playerTransform;
                found = CatDetectionTarget.Player;
            }
        }
        return found;
    }

    protected override void UpdateSightParameters()
    {
        float fovMultiplier = 1f;
        float rangeMultiplier = 1f;
        float baseRange = CatData.catDetectionRange;

        // 시간대에 따른 multiplier 적용
        var tz = DailyManager.Instance.TZ_State.Value;
        switch (tz)
        {
            case TimeZoneState.Morning: // 오전
                CatData.catDetectionRange = 0f; // 감지 없음
                break;
            case TimeZoneState.Afternoon: // 정오
                CatData.catDetectionRange = baseRange; // a
                break;
            case TimeZoneState.Evening: // 오후
                CatData.catDetectionRange = baseRange * 1.5f; // a x 1.5
                break;
            case TimeZoneState.Night: // 밤
                CatData.catDetectionRange = baseRange * 1.25f; // a x 1.25
                break;
            default:
                CatData.catDetectionRange = baseRange;
                break;
        }

        // 상태별 multiplier도 합산 적용 (예: Alert 땐 더 멀리)
        switch (perceptionState)
        {
            case MonsterPerceptionState.Idle:
                fovMultiplier = 1f;
                break;
            case MonsterPerceptionState.Search:
                fovMultiplier = 1f;
                break;
            case MonsterPerceptionState.Alert:
                fovMultiplier = 1f;
                rangeMultiplier *= 1f;
                break;  
            case MonsterPerceptionState.Combat:
                fovMultiplier = 1f;
                rangeMultiplier *= 1f;
                break;
        }

        currentFOV = data.BaseFOV * fovMultiplier;
        currentDetectionRange = data.DetectionRange * rangeMultiplier;
        Debug.Log($"고양이 감지 파라미터 갱신: FOV={currentFOV}  Range={currentDetectionRange}");
    }
    public void SetInvincible(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(InvincibleCoroutine(duration));
    }
    private IEnumerator InvincibleCoroutine(float duration)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
    }

    //public override void Move(Vector3 direction, float customSpeed = -1f)
    //{
    //    if (RB == null)
    //    {
    //        Debug.LogWarning("Rigidbody가 없습니다!");
    //        return;
    //    }
    //
    //    if (IsOutsideActionRadius())
    //        return;
    //
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion lookRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    //    }
    //
    //    // 커스텀 속도 지정, 없으면 CatData.maxMoveSpeed 사용
    //    float moveSpd = (customSpeed > 0f) ? customSpeed : CatData.chaseMoveSpeed;
    //    Vector3 targetPosition = RB.position + (direction * moveSpd * Time.deltaTime);
    //    RB.MovePosition(targetPosition);
    //}
    public override void MoveTo(Vector3 destination)
    {
        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent가 없습니다!");
            return;
        }

        agent.speed = CatData.chaseMoveSpeed;
        agent.SetDestination(destination);
    }
    public void MoveToRespawn()
    {
        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        perceptionController.ResetAlert();// 경계도 값 리셋
        SetPerceptionState(MonsterPerceptionState.Idle);
        stateMachine.ChangeState(new CatIdleState());
        // 혹은 네비메시 경로 이동, 상태 변경 등
    }

    public override void TakeDamage(int damage)
    {
    }
    protected override void Die()
    {
    }

    protected override void Phase2TryAttack()
    {
        throw new System.NotImplementedException();
    }
    protected override void Phase3TryAttack()
    {
        throw new System.NotImplementedException();
    }
    public void PlayCutsceneAnim(int cutsceneType)
    {
        switch (cutsceneType)
        {
            case 0: animator.SetTrigger("CatCutsceneA"); break; // 컷씬 1
            case 1: animator.SetTrigger("CatCutsceneB"); break; // 컷씬 2
        }
    }
}