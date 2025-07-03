using UnityEngine;

public class CatChaseState : IMonsterState
{
    private CatAI cat;
    private Transform target;

    private float mentalTickInterval = 1.0f;
    private float mentalDamage = 3.0f;
    private float mentalTickTimer = 0f;

    // �̵��ӵ� Lerp ���� ����
    private float speedLerpTimer = 0f;
    private float lerpDuration = 3f;
    private float startSpeed;
    private float targetSpeed;
    private float currentSpeed;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        if (cat == null) return;

        // ���� ��� ��Ž�� (�׻� �ֽ� Ÿ��)
        cat.RefreshBaitList();
        CatAI.CatDetectionTarget targetType = cat.GetClosestTarget(out target);

        cat.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();
        Debug.Log($"[{cat.name}] ����: CatChase ����");

        mentalTickTimer = 0f;

        // Lerp ����
        startSpeed = cat.CatData.basicMoveSpeed;   // Ž���ӵ����� ����
        targetSpeed = cat.CatData.chaseMoveSpeed;   // �����ӵ��� ��ǥ
        currentSpeed = startSpeed;
        speedLerpTimer = 0f;
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead)
            return;

        // Ÿ�� �ǽð� ��Ž��
        cat.RefreshBaitList();
        CatAI.CatDetectionTarget targetType = cat.GetClosestTarget(out target);

        if (target == null || targetType == CatAI.CatDetectionTarget.None)
        {
            cat.StateMachine.ChangeState(new CatIdleState());
            return;
        }

        // �÷��̾� ���� �� ����/���� ������ ��赵 ���(��� Alert ����)
        if (cat.CatData != null && targetType == CatAI.CatDetectionTarget.Player)
        {
            if (cat.IsPlayerMakingNoise() && cat.IsInDetectionRange(target))
            {
                cat.IncreaseAlert(cat.CatData.footstepAlertValue);
                cat.StateMachine.ChangeState(
                    cat.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert));
                return;
            }
        }

        if (speedLerpTimer < lerpDuration)
        {
            speedLerpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(speedLerpTimer / lerpDuration);
            currentSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        cat.Agent.speed = currentSpeed; 

        cat.Agent.SetDestination(target.position);

        // ���ŷ� ������ �ֱ� (�������̽� ���)
        mentalTickTimer += Time.deltaTime;
        if (mentalTickTimer >= mentalTickInterval)
        {
            var mental = target.GetComponent<IMentalStamina>();
            if (mental != null)
            {
                mental.ReduceMental(mentalDamage);
                Debug.Log($"[{cat.name}] �� ���ŷ� {mentalDamage} ���� (���� ����)");
            }
            mentalTickTimer = 0f;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] CatChase ����");
    }
}
