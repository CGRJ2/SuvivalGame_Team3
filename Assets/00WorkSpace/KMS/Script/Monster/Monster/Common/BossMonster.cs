using KMS.Monster.Interface;
using UnityEngine;

public class BossMonster : BaseMonster
{
    private BossMonsterDataSO bossData;
    public BossAttackPatternSO bossAttackPatternSO;
    public BossAttackPattern currentPattern;

    private float prevNotifiedHpPercent = 1f; // 처음엔 100%
    private int batteryChargePerSection = 20; // 10%마다 충전할 양
    private bool isCounterWindow = false;


    private BoxCollider rushHitbox;
    protected override void Start()
    {
        base.Start();
        bossData = data as BossMonsterDataSO;
        if (bossData == null)
            Debug.LogError("BossMonsterDataSO로 캐스팅 실패! data가 잘못 세팅됨.");
        if (bossAttackPatternSO != null && bossAttackPatternSO.patterns.Count > 0)
            currentPattern = bossAttackPatternSO.patterns[0];
    }
    protected override void HandleState()
    {
        float hpPercent = currentHP / data.MaxHP;

        // 10% 단위로 줄어들 때마다 충전
        if (hpPercent <= prevNotifiedHpPercent - 0.1f)
        {
            PlayerController pc = PlayerManager.Instance.instancePlayer;
            if (pc != null)
                PlayerManager.Instance.instancePlayer.Status.ChargeBattery(SuvivalSystemManager.Instance.batterySystem.RecoverAmount_BossHit);

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
            stateMachine.ChangeState(new MonsterIdleState(this));
    }
    // 오버라이드 
    public void phase2TryAttack(BossAttackPattern pattern)
    {
        // 애니메이터 속도 (패턴에서 지정, 없으면 SO 기본값)
        view.Animator.SetFloat("Phase2AttackSpeed", pattern != null ? pattern.cooldown : bossData.Phase2AnimSpeed);

        switch (pattern.shape)
        {
            case BossAttackShape.Cone:
                // 부채꼴 영역 내 타겟 탐색 및 데미지
                ApplyConeDamage(pattern);
                break;
            case BossAttackShape.Box:
                // 박스 범위 내 타겟 탐색 및 데미지
                ApplyBoxDamage(pattern);
                break;
            case BossAttackShape.Circle:
                // 원형 범위 내 타겟 탐색 및 데미지
                ApplyCircleDamage(pattern);
                break;
        }

        // 데미지/넉백 등 패턴값 우선, 없으면 SO값
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
        //view.PlayBossHealEffect(); // 회복 연출이 있으면 호출
        Debug.Log("[Boss] HP가 최대치로 회복됨");
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
        // 위치, 방향, 크기 세팅
        Vector3 boxCenter = transform.position + transform.forward * (pattern.length * 0.5f); // 앞쪽에 박스 생성
        Vector3 boxHalfExtents = new Vector3(pattern.width * 0.5f, pattern.height * 0.5f, pattern.length * 0.5f);
        Quaternion boxRotation = transform.rotation;

        // 박스 내 모든 콜라이더 탐색
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
        float angle = pattern.angle * 0.5f; // angle은 부채꼴의 "반각"

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

    // 카운트 어택용 패턴

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

    // 돌진 패턴용 히트 박스 트리거
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
