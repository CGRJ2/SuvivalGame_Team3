using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatChaseState : IMonsterState
{
    private CatAI cat;
    private Transform target;

    private float mentalTickInterval = 1.0f;
    private float mentalDamage = 3.0f;
    private float mentalTickTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        if (cat == null) return;

        target = cat.GetTarget();
        cat.GetComponent<MonsterView>()?.PlayMonsterRunAnimation();
        Debug.Log($"[{cat.name}] 상태: CatChase 진입");

        mentalTickTimer = 0f;
    }

    public void Execute()
    {
        if (target == null)
        {
            cat.StateMachine.ChangeState(new CatIdleState());
            return;
        }

        if (cat.CatData != null)
        {
            if (cat.IsPlayerMakingNoise() && cat.IsInDetectionRange(target))
            {
                cat.IncreaseAlert(cat.CatData.footstepAlertValue);
                cat.StateMachine.ChangeState(
                    cat.StateFactory.GetStateForPerception(MonsterPerceptionState.Alert));
                return;
            }
        }

        // 이하 동일
        Vector3 dir = (target.position - cat.transform.position).normalized;
        cat.Move(dir);

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

        float dist = Vector3.Distance(cat.transform.position, target.position);
        if (dist < 2f)
        {
            cat.GetComponent<MonsterView>()?.PlayMonsterAttackAnimation();
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] CatChase 종료");
    }


}

