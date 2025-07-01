using System.Collections.Generic;
using UnityEngine;

public class CatAI : BaseMonster
{
    public enum CatDetectionTarget { None, CatBait, Player }
    public CatMonsterSO CatData => data as CatMonsterSO;

    private List<Transform> baitTransforms = new List<Transform>();
    private Transform playerTransform;

    protected override void Awake()
    {
        stateFactory = new CatMonsterStateFactory(this);
        base.Awake();
    }
    protected override void Start()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        RefreshBaitList();
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
        SetPerceptionState(MonsterPerceptionState.Idle); // 혹은 Pacified 전용 Enum도 고려 가능
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

    protected override void Phase2TryAttack()
    {
        throw new System.NotImplementedException();
    }
    protected override void Phase3TryAttack()
    {
        throw new System.NotImplementedException();
    }
}