using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase3AttackState : IMonsterState
{
    private BaseMonster monster;
    private BossMonster bossMonster;
    private BossMonsterDataSO bossData;

    // 패턴별 리스트 (SO에서 받아옴)
    private List<BossAttackPattern> phase2Patterns;
    private List<BossAttackPattern> phase3Patterns;
    private List<BossAttackPattern> normalPatterns;

    private BossAttackPattern currentPattern;

    private enum AttackPhase { None, Prelude, Attack, AfterDelay }
    private AttackPhase attackPhase = AttackPhase.None;

    private float timer;

    public BossPhase3AttackState() { }

    public void Enter(BaseMonster monster)
    {
        this.monster = monster;
        bossMonster = monster as BossMonster;
        bossData = monster.data as BossMonsterDataSO;

        if (bossData == null)
        {
            Debug.LogError("BossMonsterDataSO 타입이 아님");
            return;
        }

        phase2Patterns = bossData.phase2PatternSO?.patterns;
        phase3Patterns = bossData.phase3PatternSO?.patterns;
        normalPatterns = bossData.normalPatternSO?.patterns;

        attackPhase = AttackPhase.Prelude;
        timer = 0f;

        SelectNextPattern();
        monster.GetComponent<MonsterView>()?.PlayMonsterPhase3PreludeAnimation();
    }

    private void SelectNextPattern()
    {
        // 0.0~1.0
        float rnd = Random.value;
        if (rnd < 0.3f && phase2Patterns != null && phase2Patterns.Count > 0)
        {
            // Phase2 특수 공격 (30%)
            currentPattern = phase2Patterns[Random.Range(0, phase2Patterns.Count)];
        }
        else if (rnd < 0.7f && phase3Patterns != null && phase3Patterns.Count > 0)
        {
            // Phase3 특수 공격 (40%)
            currentPattern = phase3Patterns[Random.Range(0, phase3Patterns.Count)];
        }
        else if (normalPatterns != null && normalPatterns.Count > 0)
        {
            // 일반공격 (30%)
            currentPattern = normalPatterns[Random.Range(0, normalPatterns.Count)];
        }
        else
        {
            Debug.LogWarning("[BossPhase3] No valid pattern found, fallback to first available pattern.");
            currentPattern = (phase3Patterns != null && phase3Patterns.Count > 0)
                ? phase3Patterns[0]
                : null;
        }
    }

    public void Execute()
    {
        if (monster == null || monster.IsDead) return;

        timer += Time.deltaTime;

        switch (attackPhase)
        {
            case AttackPhase.Prelude:
                if (timer >= currentPattern.preludeTime)
                {
                    attackPhase = AttackPhase.Attack;
                    timer = 0f;
                }
                break;
            case AttackPhase.Attack:
                attackPhase = AttackPhase.AfterDelay;
                timer = 0f;
                break;
            case AttackPhase.AfterDelay:
                if (timer >= currentPattern.afterDelay)
                {
                    SelectNextPattern();
                    attackPhase = AttackPhase.Prelude;
                    timer = 0f;
                    monster.GetComponent<MonsterView>()?.PlayMonsterPhase3PreludeAnimation();
                }
                break;
        }
    }

    public void Exit()
    {
        Debug.Log($"[{monster.name}] Phase3Attack 종료");
    }

    public void TriggerAttack()
    {
        if (monster == null) return;
        bossMonster.phase3TryAttack(currentPattern);
    }
}