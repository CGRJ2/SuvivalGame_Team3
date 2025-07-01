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

    private float prevNotifiedHpPercent = 1f; // ó���� 100%
    private int batteryChargePerSection = 20; // 10%���� ������ ��

    protected override void Start()
    {
        base.Start();
        bossData = data as BossMonsterDataSO;
        if (bossData == null)
            Debug.LogError("BossMonsterDataSO�� ĳ���� ����! data�� �߸� ���õ�.");
    }
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
public void phase2TryAttack(BossAttackPattern pattern)
{
    // �ִϸ����� �ӵ� (���Ͽ��� ����, ������ SO �⺻��)
    view.Animator.SetFloat("Phase2AttackSpeed", pattern != null ? pattern.cooldown : bossData.Phase2AnimSpeed);

    // ������/�˹� �� ���ϰ� �켱, ������ SO��
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
        //view.PlayBossHealEffect(); // ȸ�� ������ ������ ȣ��
        Debug.Log("[Boss] HP�� �ִ�ġ�� ȸ����");
    }
    // ĸ��ȭ
    public void phase2TryAttack() => Phase2TryAttack();
    public void phase3TryAttack() => Phase3TryAttack();

}
