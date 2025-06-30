using KMS.Monster.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class BossMonster : BaseMonster
{
    private float prevNotifiedHpPercent = 1f; // ó���� 100%
    private int batteryChargePerSection = 20; // 10%���� ������ ��
    protected override void HandleState()
    {
        float hpPercent = currentHP / data.MaxHP;

        // 10% ������ �پ�� ������ ����
        if (hpPercent <= prevNotifiedHpPercent - 0.1f)
        {
            var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatus>();
            if (player != null)
                player.ChargeBattery(batteryChargePerSection);

            prevNotifiedHpPercent -= 0.1f; // ���� 10%�� ����
        }


        if (currentHP <= 0)
        {
            stateMachine.ChangeState(new MonsterDeadState());
            return;
        }

        // ������ ���� üũ
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
    // �������̵� 
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
        //view.PlayBossHealEffect(); // ȸ�� ������ ������ ȣ��
        Debug.Log("[Boss] HP�� �ִ�ġ�� ȸ����");
    }
    // ĸ��ȭ
    public void phase2TryAttack() => Phase2TryAttack();
    public void phase3TryAttack() => Phase3TryAttack();

}
