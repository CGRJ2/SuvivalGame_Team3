using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    // ===== 애니메이션 =====
    public void PlayAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    public void PlayHitEffect()
    {
        if (hitEffect != null)
            hitEffect.Play();

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    public void PlayDeathAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Die");

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}

