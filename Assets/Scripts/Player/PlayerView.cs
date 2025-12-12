using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Animator;
    private PlayerController controller;

    float _locomotionAnimSpeed;
    [SerializeField] float _locomotionAnimLerpSpeed;

    public readonly int SpeedHash       = Animator.StringToHash("Speed");
    public readonly int IsGroundedHash  = Animator.StringToHash("IsGrounded");
    public readonly int JumpHash        = Animator.StringToHash("Jump");
    public readonly int AttackHash      = Animator.StringToHash("Attack");
    public readonly int AttackTypeHash  = Animator.StringToHash("AttackType");
    public readonly int IsRollingHash   = Animator.StringToHash("IsRolling");
    public readonly int IsLandingHash   = Animator.StringToHash("IsLanding");
    public readonly int IsFallingHash   = Animator.StringToHash("IsFalling");
    public readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
    public readonly int HitHash         = Animator.StringToHash("Hit");


    private void Awake()
    {
        Animator ??= GetComponent<Animator>();
        controller = GetComponentInParent<PlayerController>();
    }

    public void InitLocomotionAnime()
    {
        _locomotionAnimSpeed = 0f;
        Animator.SetFloat(SpeedHash, 0f);
    }

    public void UpdateLocomotionAnim(float planarSpeed, float walkSpeed, float sprintSpeed)
    {
        float target = 0f;

        // 1) 0 ~ WalkSpeed 구간  =>  0 ~ 1 (Idle ~ Walk)
        if (planarSpeed <= walkSpeed)
        {
            target = Mathf.InverseLerp(0f, walkSpeed, planarSpeed); // 0 ~ 1
        }
        // 2) WalkSpeed ~ SprintSpeed 구간  =>  1 ~ 2 (Walk ~ Sprint)
        else
        {
            if (sprintSpeed <= walkSpeed + 0.01f) sprintSpeed = walkSpeed + 0.01f; // sprint == walk 인 상황 방지용

            float t01 = Mathf.InverseLerp(walkSpeed, sprintSpeed, planarSpeed);
            target = 1f + t01; // 1 ~ 2
        }

        // 부드럽게 보간
        _locomotionAnimSpeed = Mathf.Lerp(
            _locomotionAnimSpeed,
            target,
            _locomotionAnimLerpSpeed * Time.deltaTime
        );

        Animator.SetFloat(SpeedHash, _locomotionAnimSpeed);
    }

    public void OnLandAnimationFinished()
    {
        Animator.SetBool(IsLandingHash, false);
        controller.OnLandAnimationFinished();
    }

    public void OnAttackAnimationStarted()
    {
        controller.OnAttackAnimationStarted();
    }

    public void OnAttackAnimationFinished()
    {
        controller.OnAttackAnimationFinished();
    }
}
