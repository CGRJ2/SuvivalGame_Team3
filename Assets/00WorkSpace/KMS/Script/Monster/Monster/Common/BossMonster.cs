using KMS.Monster.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class BossMonster : BaseMonster
{
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;

    private float prevNotifiedHpPercent = 1f; // 처음엔 100%
    private int batteryChargePerSection = 20; // 10%마다 충전할 양

    protected override void Start()
    {
        base.Start();
        bossData = data as BossMonsterDataSO;
        if (bossData == null)
            Debug.LogError("BossMonsterDataSO로 캐스팅 실패! data가 잘못 세팅됨.");
    }
    protected override void HandleState()
    {
        float hpPercent = currentHP / data.MaxHP;

        // 10% 단위로 줄어들 때마다 충전
        if (hpPercent <= prevNotifiedHpPercent - 0.1f)
        {
            var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatus>();
            if (player != null)
                player.ChargeBattery(batteryChargePerSection);

            prevNotifiedHpPercent -= 0.1f; // 다음 10%로 갱신
        }


        if (currentHP <= 0)
        {
            stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        // 페이즈 전이 체크
        if (currentHP < data.MaxHP * 0.5f)
        {
            if (!(stateMachine.CurrentState is BossPhase3AttackState))
                stateMachine.ChangeState(new BossPhase3AttackState());
            return;
        }
        else if (currentHP < data.MaxHP * 0.75f)
        {
            if (!(stateMachine.CurrentState is BossPhase2AttackState))
                stateMachine.ChangeState(new BossPhase2AttackState());
            return;
        }
        if (SetPerceptionState(MonsterPerceptionState.Alert))
            stateMachine.ChangeState(new BossPhase1AttackState());
        else
            stateMachine.ChangeState(new MonsterIdleState());
    }
    // 오버라이드 
public void phase2TryAttack(BossAttackPattern pattern)
{
    // 애니메이터 속도 (패턴에서 지정, 없으면 SO 기본값)
    view.Animator.SetFloat("Phase2AttackSpeed", pattern != null ? pattern.cooldown : bossData.Phase2AnimSpeed);

    // 데미지/넉백 등 패턴값 우선, 없으면 SO값
    int damage = (int)(pattern != null ? pattern.damage : bossData.Phase2AttackPower);
    float knockback = pattern != null ? pattern.range : bossData.Phase2KnockbackDistance;

    if (target != null)
    {
        var dmg = target.GetComponent<IDamagable>();
        var kb = target.GetComponent<IKnockbackable>();
        Vector3 direction = (target.position - transform.position).normalized;
        if (dmg != null) dmg.TakeDamage(damage);
        if (kb != null) kb.ApplyKnockback(direction, knockback);
    }
    view.PlayMonsterPhase2AttackAnimation();
}
    public void phase3TryAttack(BossAttackPattern pattern)
    {
        view.Animator.SetFloat("Phase3AttackSpeed", pattern != null ? pattern.cooldown : 1f);

        int damage = (int)(pattern != null ? pattern.damage : bossData.Phase3AttackPower);
        float knockback = pattern != null ? pattern.range : bossData.Phase3KnockbackDistance;

        if (target != null)
        {
            var dmg = target.GetComponent<IDamagable>();
            var kb = target.GetComponent<IKnockbackable>();
            Vector3 direction = (target.position - transform.position).normalized;
            if (dmg != null) dmg.TakeDamage(damage);
            if (kb != null) kb.ApplyKnockback(direction, knockback);
        }
        view.PlayMonsterPhase3AttackAnimation();
    }
    public void ResetBoss()
    {
        currentHP = data.MaxHP;
        //view.PlayBossHealEffect(); // 회복 연출이 있으면 호출
        Debug.Log("[Boss] HP가 최대치로 회복됨");
    }
    // 캡슐화
    public void phase2TryAttack() => Phase2TryAttack();
    public void phase3TryAttack() => Phase3TryAttack();

}
