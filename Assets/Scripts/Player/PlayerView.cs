using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Animator;
    private PlayerController controller;

    float _locomotionAnimSpeed;
    [SerializeField] float _locomotionAnimLerpSpeed;

    public string SpeedHash = "Speed";
    public string IsGroundedHash = "IsGrounded";
    public string JumpHash       = "Jump";
    public string AttackHash       = "Attack";
    public string IsRollingHash  = "IsRolling";
    public string IsLandingHash  = "IsLanding";
    public string IsFallingHash  = "IsFalling";
    public string IsCrouchingHash = "IsCrouching";
    

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
        controller.Model.IsLanding = false; // 여기만 예외로 Model에 직접 접근하는게 나을 듯? 애니메이션 이벤트니까...
        // controller 안에 OnLand 함수를 만들어서, 그 안에 Model의 플래그를 변경하는게 깔끔할 거 같긴 한데
    }

    public void OnAttackAnimationStarted()
    {

    }

    public void OnAttackAnimationFinished()
    {
        var attackState = controller.GetState(PlayerStateType.Attack) as PlayerAttackState;
        attackState.QuitAttack();
    }
}
