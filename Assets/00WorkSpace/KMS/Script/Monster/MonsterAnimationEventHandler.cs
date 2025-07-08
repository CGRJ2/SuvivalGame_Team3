using UnityEngine;

public class MonsterAnimationEventHandler : MonoBehaviour
{
    public BaseMonster monster;

    private void Awake() => Init();
    private void Init()
    {
        monster = GetComponentInParent<BaseMonster>();
    }
    public void ApplyDamage()
    {
        monster.ApplyDamage();
    }
    public void ApplyBoxDamage()
    {
        if (monster is BossMonster boss)
            boss.ApplyBoxDamage(boss.currentPattern);
    }
    public void ApplyCircleDamage()
    {
        if (monster is BossMonster boss)
            boss.ApplyCircleDamage(boss.currentPattern);
    }
    public void ApplyConeDamage()
    {
        if (monster is BossMonster boss)
            boss.ApplyConeDamage(boss.currentPattern);
    }
    public void OnCounterWindowOpen()
    {
        if (monster is BossMonster boss)
            boss.OnCounterWindowOpen();
    }
    public void OnCounterWindowClose()
    {
        if (monster is BossMonster boss)
            boss.OnCounterWindowClose();
    }
    public void OnHitboxTrigger()
    {
        if (monster is BossMonster boss)
            boss.OnHitboxTrigger();
    }
    public void OffHitboxTrigger()
    {
        if (monster is BossMonster boss)
            boss.OffHitboxTrigger();
    }
}