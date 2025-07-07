using KMS.Monster.Interface;
using UnityEngine;

public class BossMonster : BaseMonster
{
    private BossMonsterDataSO bossData;
    public BossAttackPatternSO bossAttackPatternSO;
    public BossAttackPattern currentPattern;

    private float prevNotifiedHpPercent = 1f; // ó���� 100%
    private int batteryChargePerSection = 20; // 10%���� ������ ��
    private bool isCounterWindow = false;


    private BoxCollider rushHitbox;
    protected override void Start()
    {
        base.Start();
        bossData = data as BossMonsterDataSO;
        if (bossData == null)
            Debug.LogError("BossMonsterDataSO�� ĳ���� ����! data�� �߸� ���õ�.");
        if (bossAttackPatternSO != null && bossAttackPatternSO.patterns.Count > 0)
            currentPattern = bossAttackPatternSO.patterns[0];
    }
    protected override void HandleState()
    {
        float hpPercent = currentHP / data.MaxHP;

        // 10% ������ �پ�� ������ ����
        if (hpPercent <= prevNotifiedHpPercent - 0.1f)
        {
            PlayerController pc = PlayerManager.Instance.instancePlayer;
            if (pc != null)
                PlayerManager.Instance.instancePlayer.Status.ChargeBattery(SuvivalSystemManager.Instance.batterySystem.RecoverAmount_BossHit);

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
            stateMachine.ChangeState(new MonsterIdleState(this));
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
            if (dmg != null) dmg.TakeDamage(damage, transform);
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
            if (dmg != null) dmg.TakeDamage(damage, transform);
            if (kb != null) kb.ApplyKnockback(direction, knockback);
        }
        view.PlayMonsterPhase3AttackAnimation();
    }
    public override void ResetMonsterHP()
    {
        currentHP = data.MaxHP;
        //view.PlayBossHealEffect(); // ȸ�� ������ ������ ȣ��
        Debug.Log("[Boss] HP�� �ִ�ġ�� ȸ����");
    }

    protected void Phase2TryAttack()
    {
        phase2TryAttack(null);
    }

    protected void Phase3TryAttack()
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
                dmg.TakeDamage((int)pattern.damage, transform);
            if (kb != null)
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                kb.ApplyKnockback(dir, pattern.range);
            }
        }
    }

    public void ApplyCircleDamage(BossAttackPattern pattern)
    {
        
        Vector3 center;
        switch (pattern.originType)
        {
            case CircleOriginType.Boss:
                center = transform.position;
                break;
            case CircleOriginType.Player:
                PlayerController pc = PlayerManager.Instance.instancePlayer;
                if (pc == null) return;
                center = pc.transform.position;
                break;
            default:
                center = transform.position;
                break;
        }

        float radius = pattern.range;
        Collider[] hits = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamagable>();
            var kb = hit.GetComponent<IKnockbackable>();
            if (dmg != null)
                dmg.TakeDamage((int)pattern.damage, transform);
            if (kb != null)
                kb.ApplyKnockback((hit.transform.position - center).normalized, pattern.range);
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
                    dmg.TakeDamage((int)pattern.damage, transform);
                if (kb != null)
                    kb.ApplyKnockback(dir, pattern.range);
            }
        }
    }

    // ī��Ʈ ���ÿ� ����

    public void OnCounterWindowOpen()
    {
        isCounterWindow = true;
    }
    public void OnCounterWindowClose()
    {
        isCounterWindow = false;
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null && currentPattern != null)
            target.TakeDamage((int)currentPattern.damage, transform);
    }

    public void TryCounter()
    {
        if (isCounterWindow)
        {
            Phase3TryAttack();
        }
    }

    // ���� ���Ͽ� ��Ʈ �ڽ� Ʈ����
    public void OnHitboxTrigger()
    {
        if (rushHitbox != null)
            rushHitbox.enabled = true;
    }

    public void OffHitboxTrigger()
    {
        if (rushHitbox != null)
            rushHitbox.enabled = false;
    }
}
