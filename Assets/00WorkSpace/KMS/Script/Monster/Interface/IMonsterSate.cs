using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void Enter(BaseMonster monster);
    void Execute();
    void Exit();
}
