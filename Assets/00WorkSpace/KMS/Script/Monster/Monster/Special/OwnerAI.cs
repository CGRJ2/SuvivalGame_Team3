using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerAI : BaseMonster
{
    public enum OnwerDetectionTarget { None, CatBait, Player }
    public OwnerMonsterSO OwnerData => data as OwnerMonsterSO;
    public bool IsInvincible { get; private set; } = true;
    private List<Transform> baitTransforms = new List<Transform>();
    private Transform playerTransform;
    public Transform respawnPoint;
    protected override void Awake()
    {
        stateFactory = new OwnerStateFactory(this);
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        IsInvincible = true;
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
        SetPerceptionState(MonsterPerceptionState.Idle);
        StateMachine.ChangeState(new CatPacifiedState(duration));
    }

    public OnwerDetectionTarget GetClosestTarget(out Transform closest)
    {
        closest = null;
        float minDist = float.MaxValue;
        OnwerDetectionTarget found = OnwerDetectionTarget.None;

        // 1. 미끼 먼저 탐색
        foreach (var bait in baitTransforms)
        {
            if (bait == null) continue;
            float dist = Vector3.Distance(transform.position, bait.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = bait;
                found = OnwerDetectionTarget.CatBait;
            }
        }
        // 2. 플레이어가 더 가까우면 덮어씀
        if (playerTransform != null)
        {
            float playerDist = Vector3.Distance(transform.position, playerTransform.position);
            if (playerDist < minDist)
            {
                minDist = playerDist;
                closest = playerTransform;
                found = OnwerDetectionTarget.Player;
            }
        }
        return found;
    }
    public void RefreshBaitList()
    {
        baitTransforms.Clear();
        var baits = GameObject.FindGameObjectsWithTag("OwnerBait");
        foreach (var bait in baits)
            baitTransforms.Add(bait.transform);
    }
    public void ThrowPlayer(Vector3 direction, float force)
    {
        var player = GetTarget()?.GetComponent<IThrowable>();
        if (player != null)
        {
            player.ApplyThrow(direction, force);
        }
    }
    public override void Move(Vector3 direction, float customSpeed = -1f)
    {
        if (RB == null)
        {
            Debug.LogWarning("Rigidbody가 없습니다!");
            return;
        }

        if (IsOutsideActionRadius())
            return;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // OwnerData 참조로 커스텀 속도 적용
        float moveSpd = (customSpeed > 0f) ? customSpeed : OwnerData.moveSpeed;
        Vector3 targetPosition = RB.position + (direction * moveSpd * Time.deltaTime);
        RB.MovePosition(targetPosition);
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
}