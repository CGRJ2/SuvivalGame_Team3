using UnityEngine;

public class CatChaseState : IMonsterState
{
    private CatAI cat;
    private Transform target;

    private float mentalTickInterval = 1.0f;
    private float mentalDamage = 3.0f;
    private float mentalTickTimer = 0f;

    // 이동속도 Lerp 관련 변수
    private float speedLerpTimer = 0f;
    private float lerpDuration = 3f;
    private float startSpeed;
    private float targetSpeed;
    private float currentSpeed;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        if (cat == null) return;

        // 추적 대상 재탐색 (항상 최신 타겟)
        cat.RefreshBaitList();
        CatAI.CatDetectionTarget targetType = cat.GetClosestTarget(out target);

        cat.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();
        Debug.Log($"[{cat.name}] 상태: CatChase 진입");

        mentalTickTimer = 0f;

        // Lerp 세팅
        startSpeed = cat.CatData.basicMoveSpeed;   // 탐색속도에서 시작
        targetSpeed = cat.CatData.chaseMoveSpeed;   // 추적속도가 목표
        currentSpeed = startSpeed;
        speedLerpTimer = 0f;
    }

    public void Execute()
    {
        if (cat == null || cat.IsDead)
            return;

        // 타겟 실시간 재탐색
        cat.RefreshBaitList();
        CatAI.CatDetectionTarget targetType = cat.GetClosestTarget(out target);

        if (target == null || targetType == CatAI.CatDetectionTarget.None)
        {
            cat.StateMachine.ChangeState(new CatIdleState());
            return;
        }

        // 플레이어 추적 중 소음/범위 등으로 경계도 상승(즉시 Alert 전이)
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

        // 정신력 데미지 주기 (인터페이스 기반)
        mentalTickTimer += Time.deltaTime;
        if (mentalTickTimer >= mentalTickInterval)
        {
            var mental = target.GetComponent<IMentalStamina>();
            if (mental != null)
            {
                mental.ReduceMental(mentalDamage);
                Debug.Log($"[{cat.name}] → 정신력 {mentalDamage} 감소 (추적 지속)");
            }
            mentalTickTimer = 0f;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] CatChase 종료");
    }
}
