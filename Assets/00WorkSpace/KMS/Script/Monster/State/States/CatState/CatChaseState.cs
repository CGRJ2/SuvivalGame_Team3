using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatChaseState : IMonsterState
{
    private CatAI cat;
    private Transform target;

    // 정신력 감소 주기 및 수치 (기본값, 나중에 튜닝 가능)
    private float mentalTickInterval = 1.0f;
    private float mentalDamage = 3.0f;
    private float mentalTickTimer = 0f;

    public void Enter(BaseMonster monster)
    {
        cat = monster as CatAI;
        if (cat == null) return;

        target = cat.GetTarget();
        cat.GetComponent<MonsterView>()?.PlayRunAnimation();
        Debug.Log($"[{cat.name}] 상태: CatChase 진입");

        mentalTickTimer = 0f; // 진입 시 초기화
    }

    public void Execute()
    {
        if (target == null)
        {
            cat.StateMachine.ChangeState(new CatIdleState());
            return;
        }

        Vector3 dir = (target.position - cat.transform.position).normalized;
        cat.Move(dir);

        // 정신력 감소 처리
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

        // 공격 애니메이션은 단순 연출용
        float dist = Vector3.Distance(cat.transform.position, target.position);
        if (dist < 2f)
        {
            cat.GetComponent<MonsterView>()?.PlayAttackAnimation();
        }
    }

    public void Exit()
    {
        Debug.Log($"[{cat.name}] CatChase 종료");
    }
}
