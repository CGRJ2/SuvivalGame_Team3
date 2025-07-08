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
    private float baseRotateSmooth = 2f; // ���ع��� ȸ�� �ӵ�

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
        //Debug.Log("Idle ���� ����");
    }

    public virtual void Execute()
    {
        if (monster == null || monster.IsDead) return;
        
        // ���� ���� (��ð��̸� �ڷ���)
        if (monster is Stalker_Owner stalker)
        {
            if (DailyManager.Instance.currentTimeData.TZ_State.Value == TimeZoneState.Night)
                monster.StateMachine.ChangeState(stalker.returnToBed);
        }


        switch (wanderState)
        {
            case WanderState.Idle:
                //Debug.Log("Idle-���� �̵���ġ ���ϴ� ��");

                SetRandomDestination();
                
                wanderState = WanderState.Moving;
                break;

            case WanderState.Moving:
                if (monster.Agent.isOnNavMesh)
                    monster.Agent.isStopped = false;
                //Debug.Log("Idle-�����̵� ����");
                monster.view.Animator.SetBool("IsMove", true);
                //Debug.Log($"���� �Ÿ� {monster.Agent.remainingDistance}, ���� �Ÿ� {monster.Agent.stoppingDistance}");
                if (monster.Agent.remainingDistance <= monster.Agent.stoppingDistance && !monster.Agent.pathPending)
                {
                    wanderState = WanderState.Waiting;
                    waitTimer = UnityEngine.Random.Range(data.WaitTimeMin, data.WaitTimeMax);
                }
                break;

            case WanderState.Waiting:

                //Debug.Log("Idle-���� ����");
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

        // �ü� ��鸲
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
        //Debug.Log("Idle ���� ����");

        monster.view.Animator.SetBool("IsMove", true);
        if (monster.Agent.isOnNavMesh)
            monster.Agent.isStopped = false;
        // Ư���� �� �� ���� (�ʿ� �� �߰�)
    }
}
