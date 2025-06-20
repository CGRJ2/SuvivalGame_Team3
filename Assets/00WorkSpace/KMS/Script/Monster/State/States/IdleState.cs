using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IMonsterState
{
    private BaseMonster monster;

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        // 애니메이션 변경 등
    }

    public void Execute()
    {
        // 유저 탐지 조건 등
    }

    public void Exit()
    {
        // 나가기 전 정리
    }
}

