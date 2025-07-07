using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    [Header("Components")]
    public Transform avatar;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    public Animator Animator => animator;

    private void Awake()
    {
        animator = avatar.GetComponent<Animator>();
    }

    // ===== 애니메이션 =====
    public void PlayMonsterIdleAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Idle");
    }
    public void PlayMonsterRunAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Run");
    }
    public void PlayMonsterAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }
    public void PlayMonsterPhase2PreludeAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Phase2Prelude");
    }
    public void PlayMonsterPhase3PreludeAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Phase3Prelude");
    }
    public void OnPhase2AttackTrigger()
    {
        var bossMonster = GetComponent<BossMonster>();
        if (bossMonster != null)
        {
            var phase2State = bossMonster.StateMachine.CurrentState as BossPhase2AttackState;
            if (phase2State != null)
                bossMonster.phase2TryAttack(phase2State.CurrentPattern);
        }
    }
    public void OnPhase3AttackTrigger()
    {
        var bossMonster = GetComponent<BossMonster>();
        if (bossMonster != null)
        {
            var phase3State = bossMonster.StateMachine.CurrentState as BossPhase3AttackState;
            if (phase3State != null)
                bossMonster.phase3TryAttack(phase3State.CurrentPattern);
        }
    }
    public void PlayMonsterPhase2AttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Phase2Attack");
    }
    public void PlayMonsterPhase3AttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Phase3Attack");
    }
    public void PlayMonsterHitEffect()
    {
        if (hitEffect != null)
            hitEffect.Play();

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }
    public void PlayMonsterHitAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Hit");
    }
    public void PlayMonsterSuspiciousAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Suspicious");
    }
    public void PlayMonsterDeathAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Die");

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }

    public void PlayMonsterStaggerAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Stagger");
    }

    public void PlayMonsterCautiousWalkAnimation()
    {
        if (animator != null)
            animator.SetTrigger("CautiousWalk");
    }

    public void PlayMonsterPacifyAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Pacify");
    }

    public void PlayMonsterGrabThrowAnimation()
    {
        if (animator != null)
            animator.SetTrigger("GrabThrow");
    }
    public void PlayMonsterSleepAnimation()
    {
        if (animator != null)
            animator.SetTrigger("GrabThrow");
    }



    // ===== 사운드 =====

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}

