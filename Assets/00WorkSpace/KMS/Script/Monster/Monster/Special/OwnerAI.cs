using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwnerAI : BaseMonster
{
    public enum OwnerDetectionTarget { None, OwnerBait, Player }
    public BaseMonsterData OwnerData;

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
        //StateMachine.ChangeState(StateFactory.CreateIdleState());
        PlayerController pc = PlayerManager.Instance.instancePlayer;
        playerTransform = pc.transform;
        //RefreshBaitList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                //player.StartCatCutscene(this); // this = Owner �ν��Ͻ�
                // (���⼭�� �ƹ� �ƾ� ���� ����)
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
        // �ܺ� �ڱ�(������ ��)���� ���� ����ȭ ���� ����
        SetPerceptionState(MonsterPerceptionState.Idle);
        StateMachine.ChangeState(new CatPacifiedState(duration));
    }

    public OwnerDetectionTarget GetClosestTarget(out Transform closest)
    {
        closest = null;
        float minDist = float.MaxValue;
        OwnerDetectionTarget found = OwnerDetectionTarget.None;

        // 1. �̳� ���� Ž��
        foreach (var bait in baitTransforms)
        {
            if (bait == null) continue;
            float dist = Vector3.Distance(transform.position, bait.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = bait;
                found = OwnerDetectionTarget.OwnerBait;
            }
        }
        // 2. �÷��̾ �� ������ ���
        if (playerTransform != null)
        {
            float playerDist = Vector3.Distance(transform.position, playerTransform.position);
            if (playerDist < minDist)
            {
                minDist = playerDist;
                closest = playerTransform;
                found = OwnerDetectionTarget.Player;
            }
        }
        return found;
    }
    /*public void RefreshBaitList()
    {
        baitTransforms.Clear();
        var baits = GameObject.FindGameObjectsWithTag("OwnerBait");
        foreach (var bait in baits)
            baitTransforms.Add(bait.transform);
    }*/
    //public void ThrowPlayer(Vector3 direction, float force)
    //{
    //    var player = GetTarget()?.GetComponent<IThrowable>();
    //    if (player != null)
    //    {
    //        player.ApplyThrow(direction, force);
    //    }
    //}
    //public override void Move(Vector3 direction, float customSpeed = -1f)
    //{
    //    if (RB == null)
    //    {
    //        Debug.LogWarning("Rigidbody�� �����ϴ�!");
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
    //    // OwnerData ������ Ŀ���� �ӵ� ����
    //    float moveSpd = (customSpeed > 0f) ? customSpeed : OwnerData.moveSpeed;
    //    Vector3 targetPosition = RB.position + (direction * moveSpd * Time.deltaTime);
    //    RB.MovePosition(targetPosition);
    //}
    
    public void MoveToRespawn()
    {
        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        perceptionController.ResetAlert();// ��赵 �� ����
        SetPerceptionState(MonsterPerceptionState.Idle);
        stateMachine.ChangeState(new CatIdleState());
        // Ȥ�� �׺�޽� ��� �̵�, ���� ���� ��
    }


    protected override void OnDisable()
    {
        DailyManager.Instance.currentTimeData.TZ_State.Unsubscribe(OnTimeZoneChanged);
    }
    private void OnTimeZoneChanged(TimeZoneState newState)
    {
        Debug.Log("���� �ð��� �����: " + newState);

        if (newState == TimeZoneState.Night)
        {
            MoveToRespawn();
            StateMachine.ChangeState(new OwnerSleepState());
            Debug.Log("[Owner] , ���� ���� ����");
        }
    }
    public override void TakeDamage(float damage, Transform attackerTransform)
    {
    }
    protected override void Die()
    {
    }

    public void PlayCutsceneAnim(int cutsceneType)
    {
        switch (cutsceneType)
        {
            case 0: view.Animator.SetTrigger("OwnerCutsceneA"); break; // �ƾ�
        }
    }
}