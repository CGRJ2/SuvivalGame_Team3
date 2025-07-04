using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerAttackState : IMonsterState
{
    private BaseMonster monster;
    private Transform target;
    private float attackTimer = 0f;
    private bool hasThrown = false;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        this.target = monster.GetTarget();
        hasThrown = false;
        attackTimer = 0f;

        monster.SetPerceptionState(MonsterPerceptionState.Alert);
        monster.GetComponent<MonsterView>()?.PlayMonsterGrabThrowAnimation(); // 애니메이션 연결

        Debug.Log($"[{monster.name}] 상태: OwnerAttack 진입 (잡기 시작)");
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        attackTimer += Time.deltaTime;

        // 예시: 1초 후에 던지는 이벤트 발생
        if (!hasThrown && attackTimer >= 1.0f)
        {
            ThrowTarget();
            hasThrown = true;
        }

        // 공격 후 일정 시간 뒤 Idle 전이
        if (attackTimer >= 2.0f)
        {
            monster.StateMachine.ChangeState(new MonsterIdleState(monster));
        }
    }

    private void ThrowTarget()
    {
        if (target == null) return;

        Vector3 throwDirection = monster.transform.forward + Random.insideUnitSphere * 0.3f;
        throwDirection.y = 0.5f; // 위쪽 성분 추가

        // 플레이어에 IThrowable이 구현되어 있을 경우
       IThrowable throwable = target.GetComponent<IThrowable>();
       if (throwable != null)
       {
           float throwForce = 7f; // 임의 값 (기획에 따라 조정)
           throwable.ApplyThrow(throwDirection.normalized, throwForce);
           Debug.Log($"[{monster.name}] 플레이어를 던짐 → 방향: {throwDirection.normalized}, 힘: {throwForce}");
       }
       else
       {
           Debug.LogWarning($"[{monster.name}] 대상에 IThrowable 미구현");
       }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] OwnerAttack 종료");
    }
}