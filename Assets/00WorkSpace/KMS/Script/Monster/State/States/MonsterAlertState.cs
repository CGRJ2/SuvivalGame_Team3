using UnityEngine;

public class MonsterAlertState : IMonsterState
{
    private BaseMonster monster;
    private float alertTimer = 0f;
    private float maxAlertDuration = 6f;
    private float returnTriggerRatio = 0.95f;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        monster.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();
        //Debug.Log($"[{monster.name}] ����: Alert ����");
    }

    public void Execute()
    {
        // ���� ���� üũ
        monster.UpdateAttackRange();

        // (���� ���� Alert ���� ����)
        if (monster.checkTargetVisible)

            monster.IncreaseAlert(15f);

        // Ÿ���� ������ Ÿ�� ��ġ�� �����̵�
        if (monster.GetTarget() != null)
        {
            monster.Agent.SetDestination(monster.GetTarget().position);
        }

        // ���� ������ �÷��̾ �ִٸ� ���ݻ��·� ��ȯ
        if (monster.playerInRange != null)
        {
            var attackState = monster.CreateAttackState();
            monster.StateMachine.ChangeState(attackState);
            return;
        }


        alertTimer += Time.deltaTime;
        if (alertTimer >= maxAlertDuration)
        {
            var nextState = monster.StateFactory.GetStateForPerception(monster.GetCurrentPerceptionState());
            monster.StateMachine.ChangeState(nextState);
            //Debug.Log($"[{monster.name}] Alert �ð� ���� �� ���� ����: {nextState.GetType().Name}");
        }


        if (monster.AlertLevel < monster.AlertThreshold_Low)
        {
            monster.StateMachine.ChangeState(new MonsterIdleState(monster));
            //Debug.Log($"[{monster.name}] ��赵 �϶� �� Idle ����");
        }
    }

    public void Exit()
    {
        //Debug.Log($"[{monster.name}] Alert ���� ����");
    }
}