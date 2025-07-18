using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerIdleState : MonsterIdleState, IMonsterState
{
    public OwnerIdleState(BaseMonster monster) : base(monster) { }
    private float idleDuration;
    private float idleTimer;

    public override void Enter(BaseMonster monster)
    {
        base.Enter(monster);
        //this.monster = monster;
        //idleTimer = 0f;
        //idleDuration = Random.Range(3f, 5f); // 어린이라 가만히 있지를 못함.

        monster.GetComponent<MonsterView>()?.PlayMonsterIdleAnimation();
        //Debug.Log($"[{monster.name}] 상태: OwnerIdle 진입 (정지)");
    }
    public override void Execute()
    {
        base.Execute();
        //if (monster == null || monster.IsDead) return;

        /*idleTimer += Time.deltaTime;

        // 플레이어를 감지하면 경계도 상승
        if (monster.checkTargetVisible)
            monster.IncreaseAlert(40f);

        // 시간이 지나면 상태 전이 판단
        if (idleTimer >= idleDuration)
        {
            MonsterPerceptionState current = monster.GetCurrentPerceptionState();
            IMonsterState next = monster.StateFactory.GetStateForPerception(current);
            Debug.Log(next);
            if (next != this)
            {
                monster.StateMachine.ChangeState(next);
            }
            else
            {
                idleTimer = 0f;
                idleDuration = Random.Range(1f, 3f);
            }
        }*/
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"[{monster.name}] OwnerIdle 종료");
    }
}
