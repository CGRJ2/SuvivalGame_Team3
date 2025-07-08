using UnityEngine;
using UnityEngine.AI;

public class MonsterIdleState : IMonsterState
{
    enum WanderState { Idle, Moving, Waiting }
    private WanderState wanderState = WanderState.Idle;
    private float waitTimer = 0f;
    protected BaseMonster monster;
    private BaseMonsterData data;
    private float timer;
    private float lookDuration;
    private Quaternion baseRotation;
    private Quaternion baseRotationTarget;
    private float baseRotateSmooth = 2f; // 기준방향 회전 속도

    public MonsterIdleState(BaseMonster monster)
    {
        this.monster = monster;
    }
    public virtual void Enter(BaseMonster monster)
    {
        this.monster = monster;
        this.data = monster.data;
        wanderState = WanderState.Waiting;
        waitTimer = 0f;
        timer = 0f;
        monster.ResetAlert();

        if (monster.Agent != null)
        {
            monster.Agent.ResetPath();
            monster.Agent.isStopped = false;
            monster.Agent.speed = data.MoveSpeed;
            monster.Agent.enabled = true;
        }

        float startY = monster.transform.rotation.eulerAngles.y;
        baseRotation = Quaternion.Euler(0, startY, 0);
        baseRotationTarget = baseRotation;

        lookDuration = UnityEngine.Random.Range(1.5f, 3f);

        monster.ResetMonsterHP();

        monster.view.Animator.SetTrigger("Idle");
        //Debug.Log("Idle 상태 진입");
    }

    public virtual void Execute()
    {
        if (monster == null || monster.IsDead) return;
        
        // 주인 전용 (밤시간이면 자러감)
        if (monster is Stalker_Owner stalker)
        {
            if (DailyManager.Instance.currentTimeData.TZ_State.Value == TimeZoneState.Night)
                monster.StateMachine.ChangeState(stalker.returnToBed);
        }


        switch (wanderState)
        {
            case WanderState.Idle:
                //Debug.Log("Idle-다음 이동위치 정하는 중");

                SetRandomDestination();
                
                wanderState = WanderState.Moving;
                break;

            case WanderState.Moving:
                if (monster.Agent.isOnNavMesh)
                    monster.Agent.isStopped = false;
                //Debug.Log("Idle-랜덤이동 상태");
                monster.view.Animator.SetBool("IsMove", true);
                //Debug.Log($"남은 거리 {monster.Agent.remainingDistance}, 기준 거리 {monster.Agent.stoppingDistance}");
                if (monster.Agent.remainingDistance <= monster.Agent.stoppingDistance && !monster.Agent.pathPending)
                {
                    wanderState = WanderState.Waiting;
                    waitTimer = UnityEngine.Random.Range(data.WaitTimeMin, data.WaitTimeMax);
                }
                break;

            case WanderState.Waiting:

                //Debug.Log("Idle-정지 상태");
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    wanderState = WanderState.Idle;
                }
                else
                {
                    monster.view.Animator.SetBool("IsMove", false);
                    monster.Agent.isStopped = true;
                }
                    break;
        }

        // 시선 흔들림
        timer += Time.deltaTime;
        baseRotation = Quaternion.Slerp(baseRotation, baseRotationTarget, Time.deltaTime * baseRotateSmooth);
        float angleOffset = Mathf.Sin(timer * Mathf.PI / lookDuration) * 20f;
        Quaternion targetRotation = baseRotation * Quaternion.Euler(0f, angleOffset, 0f);

        if (!monster.Agent.hasPath || monster.Agent.velocity.sqrMagnitude < 0.01f)
        {
            monster.transform.rotation = Quaternion.Slerp(
                monster.transform.rotation,
                targetRotation,
                Time.deltaTime * 3f
            );
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = monster.transform.position + (UnityEngine.Random.insideUnitSphere * monster.ActionRadius);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, monster.ActionRadius, NavMesh.AllAreas))
            monster.Agent.SetDestination(hit.position);
    }

    public virtual void Exit()
    {
        //Debug.Log("Idle 상태 나감");

        monster.view.Animator.SetBool("IsMove", true);
        if (monster.Agent.isOnNavMesh)
            monster.Agent.isStopped = false;
        // 특별히 할 일 없음 (필요 시 추가)
    }
}
