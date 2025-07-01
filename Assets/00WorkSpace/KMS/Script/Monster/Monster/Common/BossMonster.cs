using KMS.Monster.Interface;
using UnityEngine;

public class BossMonster : BaseMonster
{
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

        switch (pattern.shape)
        {
            case BossAttackShape.Cone:
                // ��ä�� ���� �� Ÿ�� Ž�� �� ������
                ApplyConeDamage(pattern);
                break;
            case BossAttackShape.Box:
                // �ڽ� ���� �� Ÿ�� Ž�� �� ������
                ApplyBoxDamage(pattern);
                break;
            case BossAttackShape.Circle:
                // ���� ���� �� Ÿ�� Ž�� �� ������
                ApplyCircleDamage(pattern);
                break;
        }

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
        view.Animator.SetFloat("Phase3AttackSpeed", pattern != null ? pattern.cooldown : bossData.Phase3AnimSpeed);

        switch (pattern.shape)
        {
            case BossAttackShape.Box:
                ApplyBoxDamage(pattern);
                break;
            case BossAttackShape.Cone:
                ApplyConeDamage(pattern);
                break;
            case BossAttackShape.Circle:
                ApplyCircleDamage(pattern);
                break;
        }

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

    protected override void Phase2TryAttack()
    {
        phase2TryAttack(null);
    }

    protected override void Phase3TryAttack()
    {
        phase3TryAttack(null);
    }

    public void ApplyBoxDamage(BossAttackPattern pattern)
    {
        // ��ġ, ����, ũ�� ����
        Vector3 boxCenter = transform.position + transform.forward * (pattern.length * 0.5f); // ���ʿ� �ڽ� ����
        Vector3 boxHalfExtents = new Vector3(pattern.width * 0.5f, pattern.height * 0.5f, pattern.length * 0.5f);
        Quaternion boxRotation = transform.rotation;

        // �ڽ� �� ��� �ݶ��̴� Ž��
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, boxRotation, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamagable>();
            var kb = hit.GetComponent<IKnockbackable>();
            if (dmg != null)
                dmg.TakeDamage((int)pattern.damage);
            if (kb != null)
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                kb.ApplyKnockback(dir, pattern.range);
            }
        }
    }

    public void ApplyCircleDamage(BossAttackPattern pattern)
    {
        Vector3 center = transform.position + transform.forward * pattern.range; // ���� �� �߽�(����)
        float radius = pattern.range;
        Collider[] hits = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamagable>();
            var kb = hit.GetComponent<IKnockbackable>();
            if (dmg != null)
                dmg.TakeDamage((int)pattern.damage);
            if (kb != null)
                kb.ApplyKnockback((hit.transform.position - transform.position).normalized, pattern.range);
        }
    }

    public void ApplyConeDamage(BossAttackPattern pattern)
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;
        float range = pattern.range;
        float angle = pattern.angle * 0.5f; // angle�� ��ä���� "�ݰ�"

        Collider[] hits = Physics.OverlapSphere(origin, range, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            Vector3 dir = (hit.transform.position - origin).normalized;
            float dot = Vector3.Dot(forward, dir);
            float hitAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            if (hitAngle <= angle)
            {
                var dmg = hit.GetComponent<IDamagable>();
                var kb = hit.GetComponent<IKnockbackable>();
                if (dmg != null)
                    dmg.TakeDamage((int)pattern.damage);
                if (kb != null)
                    kb.ApplyKnockback(dir, pattern.range);
            }
        }
    }
}
