using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEventHandler : MonoBehaviour
{
    protected BaseMonster monster;
    protected BossMonster bossMonster;

    public void ApplyDamage()
    {
        monster.ApplyDamage();
    }
    public void ApplyBoxDamage()
    {
        bossMonster.ApplyBoxDamage(bossMonster.currentPattern);
    }
    public void ApplyCircleDamage()
    {
        bossMonster.ApplyCircleDamage(bossMonster.currentPattern);
    }
    public void ApplyConeDamage()
    {
        bossMonster.ApplyConeDamage(bossMonster.currentPattern);
    }
    public void OnCounterWindowOpen() => bossMonster.OnCounterWindowOpen();
    public void OnCounterWindowClose() => bossMonster.OnCounterWindowClose();
    public void OnHitboxTrigger() => bossMonster.OnHitboxTrigger();
    public void OffHitboxTrigger() => bossMonster.OffHitboxTrigger();
}
