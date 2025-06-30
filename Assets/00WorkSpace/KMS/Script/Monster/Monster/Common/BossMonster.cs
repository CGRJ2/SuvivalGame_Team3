using KMS.Monster.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class BossMonster : BaseMonster
{
    private float prevNotifiedHpPercent = 1f; // 처음엔 100%
    private int batteryChargePerSection = 20; // 10%마다 충전할 양
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
    protected override void Phase2TryAttack()
    {
        view.Animator.SetFloat("Phase2AttackSpeed", data.Phase2AnimSpeed);
        int damage = data.Phase2AttackPower;
        float knockback = data.Phase2KnockbackDistance;
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
    protected override void Phase3TryAttack()
    {
        view.Animator.SetFloat("Phase3AttackSpeed", data.Phase3AnimSpeed);
        int damage = data.Phase3AttackPower;
        float knockback = data.Phase3KnockbackDistance;
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
