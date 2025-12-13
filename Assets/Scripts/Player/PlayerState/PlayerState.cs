using System;
using UnityEngine;

// RootState 
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController controller;
    protected PlayerModel Status => controller.Model;
    protected PlayerView View => controller.View;
    protected PlayerState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { Debug.Log($"{stateMachine.CurState}상태 진입"); }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void UpdateLogic() { }
    public virtual void FixedUpdateLogic() { }
}

public abstract class PlayerAliveState : PlayerState
{
    protected PlayerAliveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Status.IsDead)
        {
            // TODO: DeadState로 전환
        }
    }
}

#region Alive-SubState
public abstract class PlayerGroundedState : PlayerAliveState
{
    protected PlayerGroundedState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // 지상에 들어올 때 공통 처리
        Status.IsGrounded = true;

        View.Animator.SetBool(View.IsGroundedHash, true);
        View.Animator.SetBool(View.IsFallingHash, false);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        var input = controller.Input;
        // 1) 더 이상 땅이 아니면 => 떨어지기 시작
        if (!controller.cc.IsGroundedSensor)    // 실제 땅 여부 체크
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Fall]);
            return;
        }

        // 2) 구르기 입력 처리
        if (input.RollPressed && !Status.IsRolling)
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Roll]);
            return;
        }

        // 3) 점프 입력 처리
        if (input.JumpPressed && !Status.IsRolling && !Status.IsLanding)
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Jump]);
            return;
        }

        // 4) 공격
        if (input.AttackPressed && !Status.IsRolling && !Status.IsLanding)
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Attack]);
            return;
        }

        // 5) 앉기 입력 처리 (상태 전환 없음)
        HandleCrouch(input);
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();
    }

    // 앉음 상태 처리
    protected void HandleCrouch(PlayerInputData input)
    {
        bool wantCrouch = input.CrouchHeld;

        // 1) 앉기 시작
        if (wantCrouch && !Status.IsCrouching)
        {
            Status.IsCrouching = true;
            controller.cc.SetColliderCrouch();
            View.Animator.SetBool(View.IsCrouchingHash, true);
        }
        // 2) 일어나기
        else if (!wantCrouch && Status.IsCrouching)
        {
            // 머리 위 막혀 있으면 그대로 유지
            if (controller.cc.IsHeadBlockedSensor)
                return;

            Status.IsCrouching = false;
            controller.cc.SetColliderDefault();
            View.Animator.SetBool(View.IsCrouchingHash, false);
        }
    }
}

public abstract class PlayerAirborneState : PlayerAliveState
{
    protected PlayerAirborneState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // 공중 상태에 들어올 때 공통 처리
        
        // Crouch 상태였다면 Crouch 해제
        if (Status.IsCrouching)
        {
            Status.IsCrouching = false;
            controller.cc.SetColliderDefault();
            View.Animator.SetBool(View.IsCrouchingHash, false);
        }

        Status.IsGrounded = false;
        View.Animator.SetBool(View.IsGroundedHash, false);
        View.Animator.SetBool(View.IsLandingHash, false); // 공중 상태 진입 직후에는 착지 플래그 초기화
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        var rb = controller.rb;

        // 1) 바닥에 착지했으면 => Idle / Move 로 복귀
        if (Status.IsGrounded)
        {
            Vector2 move = controller.Input.Move;
            if (move.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(controller.StateDic[PlayerStateType.Move]);
            else
                stateMachine.ChangeState(controller.StateDic[PlayerStateType.Idle]);

            return;
        }

        // 2) 떨어지는 중이라면 => Fall 상태 전환
        ///     y속도가 <= 0일 때 && 점프 어택 모션이 끝났을 때
        if (rb.velocity.y <= 0f && !(this is PlayerFallState)) // [점프 어택] 모션이 끝났을 때 조건부 추가 필요
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Fall]);
            return;
        }
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();
    }
}
#endregion

#region Alive/Grounded-SubState
// Grounded 하위 계층 상태들
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // 공중/죽음/피격 등 상위 상태 전환 먼저 처리
        if (stateMachine.CurState != this) return;

        Vector2 move = controller.Input.Move;

        // 입력 들어오면 Move 상태로
        if (move.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Move]);
        }
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();
        var rb = controller.rb;

        Vector3 v = rb.velocity;
        v.x = 0f;
        v.z = 0f;
        rb.velocity = v;

        var locoModel = controller.Model.Locomotion;
        View.UpdateLocomotionAnim(0f, locoModel.WalkSpeed, locoModel.SprintSpeed);
    }
    public override void HandleInput()
    {
        base.HandleInput();
    }
}
public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // 공중/죽음/피격 등 상위 상태 전환 먼저 처리
        if (stateMachine.CurState != this) return;

        // 이동입력 없으면 Idle 전환
        Vector2 move = controller.Input.Move;
        if (move.sqrMagnitude <= 0.01f)
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Idle]);
        }
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();
        var rb = controller.rb;
        var input = controller.Input;
        var locoModel = controller.Model.Locomotion;

        Vector2 moveInput = input.Move;
        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        Transform cam = Manager.camera.TpsViewCamera.transform;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirWorld = camForward * moveInput.y + camRight * moveInput.x;

        // Crouch / Sprint 에 따라 속도 선택
        float speed;
        if (Status.IsCrouching)
            speed = locoModel.CrouchSpeed;
        else if (input.SprintHeld)
            speed = locoModel.SprintSpeed;
        else
            speed = locoModel.WalkSpeed;

        Vector3 vel = moveDirWorld * speed;
        vel.y = rb.velocity.y;
        rb.velocity = vel;

        Transform avatarTrans = controller.View.transform;
        if (moveDirWorld.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirWorld);
            avatarTrans.rotation = Quaternion.Slerp(
                avatarTrans.rotation,
                targetRot,
                locoModel.RotateSpeed * Time.fixedDeltaTime);
        }


        // 실제 XZ 속도
        float planarSpeed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        // Animator에 전달
        View.UpdateLocomotionAnim(planarSpeed, locoModel.WalkSpeed, locoModel.SprintSpeed);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }
}
public class PlayerRollState : PlayerGroundedState
{
    private float _rollDuration = 0.5f;    // 구르기 전체 시간
    private float _rollSpeed = 8f;         // 구르기 속도
    private float _elapsed;
    private Vector3 _rollDirWorld;

    bool IsInvincible => _elapsed >= 0.1f && _elapsed <= 0.35f; // 구르기 상태 중 무적 시간

    public PlayerRollState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        if (stateMachine.CurState != this) return;

        // Crouch 상태였다면 일어나고 구르기
        if (Status.IsCrouching)
        {
            Status.IsCrouching = false;
            controller.cc.SetColliderDefault(); // 구르기 시에 Stand 콜라이더로 만드는게 맞을까? 구르기용 히트박스를 새로 정해둬야할듯
            View.Animator.SetBool(View.IsCrouchingHash, false);
        }

        _elapsed = 0f;

        var rb = controller.rb;
        var input = controller.Input;
        var loco = controller.Model;

        // 1) 구르기 방향 결정
        Vector2 moveInput = input.Move;
        Transform cam = Manager.camera.TpsViewCamera.transform;
        Transform avatarTrans = controller.View.transform;

        if (moveInput.sqrMagnitude < 0.01f)
        {
            // 입력 없으면 현재 바라보는 방향으로 구르기
            _rollDirWorld = avatarTrans.forward;
        }
        else
        {
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            _rollDirWorld = camForward * moveInput.y + camRight * moveInput.x;
            _rollDirWorld.Normalize();
        }

        // 2) 캐릭터 방향을 롤 방향으로 빠르게 맞추기
        if (_rollDirWorld.sqrMagnitude > 0.0001f)
        {
            avatarTrans.rotation = Quaternion.LookRotation(_rollDirWorld);
        }

        // 3) 애니메이션 파라메터 적용
        /// Landing 도중 구르기 시, Landing 강제 종료
        Status.IsLanding = false;
        View.Animator.SetBool(View.IsLandingHash, false);
        
        Status.IsRolling = true;
        View.Animator.SetBool(View.IsRollingHash, true);
    }
    public override void Exit()
    {
        base.Exit();

        // 롤 끝나면 XZ 속도 정리 (원하면 약간만 남겨도 됨)
        var rb = controller.rb;
        Vector3 v = rb.velocity;
        v.x = 0f;
        v.z = 0f;
        rb.velocity = v;

        View.Animator.SetBool(View.IsRollingHash, false);
        Status.IsRolling = false;
        Status.IsInvincible = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _elapsed += Time.deltaTime;

        // 구르기 도중 무적 여부 Model에 반영
        Status.IsInvincible = IsInvincible;

        // 롤 시간 끝나면 Idle/Move로 복귀
        if (_elapsed >= _rollDuration)
        {
            // 입력 있으면 Move, 아니면 Idle
            if (controller.Input.Move.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(controller.StateDic[PlayerStateType.Move]);
            else
                stateMachine.ChangeState(controller.StateDic[PlayerStateType.Idle]);
        }
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();

        var rb = controller.rb;

        // 구르는 동안에는 일정 속도 유지
        Vector3 vel = _rollDirWorld * _rollSpeed;
        vel.y = rb.velocity.y; // 중력/점프 Y속도 유지
        rb.velocity = vel;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // 구르기 동안엔 다른 입력 금지? 
    }
}
#endregion

#region Alive/Airborn-SubState
public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        var rb = controller.rb;
        var locoModel = controller.Model.Locomotion;

        // 점프 시 기존 y속도 초기화 후 점프력 적용
        Vector3 v = rb.velocity;
        v.y = 0f;
        rb.velocity = v;

        float jumpForce = locoModel.JumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        // 애니메이션 트리거
        View.Animator.ResetTrigger(View.JumpHash);
        View.Animator.SetTrigger(View.JumpHash);
        View.Animator.SetBool(View.IsFallingHash, false);
        View.Animator.SetBool(View.IsLandingHash, false);
    }

    public override void Exit()
    {
        base.Exit();
        View.Animator.ResetTrigger(View.JumpHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        // 공중/죽음/피격 등 상위 상태 전환 먼저 처리
        if (stateMachine.CurState != this) return;
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();

    }
    public override void HandleInput()
    {
        base.HandleInput();
    }
}
public class PlayerFallState : PlayerAirborneState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        View.Animator.SetBool(View.IsFallingHash, true);
    }

    public override void Exit()
    {
        base.Exit();
        View.Animator.SetBool(View.IsFallingHash, false);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // 공중/죽음/피격 등 상위 상태 전환 먼저 처리
        if (stateMachine.CurState != this) return;

        // 장시간 낙하 시 데미지? 필요하면 넣자. 이곳에서 시간 측정해서 데미지 연산
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();

        var rb = controller.rb;
        var cc = controller.cc;

        // 낙하 중 일때만 착지 판정 주기
        if (cc.IsGroundedSensor && rb.velocity.y <= 0f)
        {
            Status.IsGrounded = true;
            Status.IsLanding = true;

            // 착지 애니메이션 플래그 On
            View.Animator.SetBool(View.IsLandingHash, true);
            View.Animator.SetBool(View.IsFallingHash, false);
        }

    }
    public override void HandleInput()
    {
        base.HandleInput();
    }
}
#endregion

#region Alive/Attack-SubState


public class PlayerAttackState : PlayerAliveState
{
    public PlayerAttackState(PlayerController controller, PlayerStateMachine stateMachine)
        : base(controller, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        if (stateMachine.CurState != this) return;

        // 앉은 상태였다면 일어나고 공격
        if (Status.IsCrouching)
        {
            Status.IsCrouching = false;
            controller.cc.SetColliderDefault();
            View.Animator.SetBool(View.IsCrouchingHash, false);
        }

        Status.IsControllLocked = true;

        var attackData = Status.Combat.GetAttackData();
        controller.CurrentHitbox.Configure(attackData.Damage, attackData.Knockback, attackData.HitStun, 0f);  // 몬스터는 무적 시간 없음(0f)
        Debug.Log($"공격력: {attackData.Damage}");

        // 이동 정지 (y속도는 유지)
        var rb = controller.rb;
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

        // 이동 애니메이션 파라메터 초기화
        View.InitLocomotionAnime();

        View.Animator.SetInteger(View.AttackTypeHash, (int)attackData.AttackType);
        View.Animator.SetTrigger(View.AttackHash);
    }
    public override void Exit()
    {
        base.Exit();
        Status.IsControllLocked = false;
        controller.CurrentHitbox.SetActive(false); // 혹시 켜져 있으면 꺼주기
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (stateMachine.CurState != this) return;

        // 공격 중에 난간에서 떨어지면 => Fall
        if (!controller.cc.IsGroundedSensor)
        {
            Status.IsGrounded = false;
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Fall]);
            return;
        }
    }

    public void QuitAttack()
    {
        // 이동 입력 있으면 Move, 없으면 Idle
        var input = controller.Input;
        if (input.Move.sqrMagnitude > 0.01f)
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Move]);
        else
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Idle]);
    }

    public override void FixedUpdateLogic()
    {
        // 공격 중에는 이동 물리 추가 시 이곳에. 공격 할 때 살짝 전진하는 느낌?
    }
}


#endregion

#region Alive/Hit-SubState

public class PlayerHitState : PlayerAliveState
{
    private float _remainStun;
    private float _remainInvincible;
    private HitInfo _cashedHitInfo;
    public PlayerHitState(PlayerController c, PlayerStateMachine sm) : base(c, sm) { }

    public override void Enter()
    {
        base.Enter();
        if (stateMachine.CurState != this) return;

        var hit = _cashedHitInfo;  // Controller가 저장해둔 hit

        // 피격 후 경직 & 무적 시간
        _remainStun = Mathf.Max(0.05f, hit.HitStun); // 최소 0.05초
        _remainInvincible = hit.IFrame;

        if (_remainInvincible > 0f)
            Status.IsInvincible = true;

        // 컨트롤 잠금
        Status.IsControllLocked = true;

        // 넉백 1회 적용
        ApplyKnockback(hit);

        // 애니 재생
        View.Animator.ResetTrigger(View.HitHash);
        View.Animator.SetTrigger(View.HitHash);
    }

    private void ApplyKnockback(HitInfo hit)
    {
        var rb = controller.rb;

        Vector3 dir = (rb.position - hit.HitPoint);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) dir = -controller.transform.forward;
        dir.Normalize();

        rb.velocity = new Vector3(0f, rb.velocity.y, 0f); // xz 초기화
        rb.AddForce(dir * hit.KnockbackPower, ForceMode.VelocityChange);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (stateMachine.CurState != this) return;

        // 무적 타이머 감소
        if (_remainInvincible > 0f)
        {
            _remainInvincible -= Time.deltaTime;
            if (_remainInvincible <= 0f)
                Status.IsInvincible = false;
        }

        // 경직 타이머 감소
        _remainStun -= Time.deltaTime;
        if (_remainStun > 0f)
            return;

        // 경직 끝 => 상태 복귀
        Status.IsControllLocked = false;

        // 복귀 규칙
        if (!controller.cc.IsGroundedSensor)
        {
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Fall]);
            return;
        }

        var input = controller.Input;
        if (input.Move.sqrMagnitude > 0.01f)
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Move]);
        else
            stateMachine.ChangeState(controller.StateDic[PlayerStateType.Idle]);
    }

    public override void Exit()
    {
        base.Exit();
        Status.IsControllLocked = false;

        // 혹시 무적 상태가 남아있다면 정리
        Status.IsInvincible = false;
    }

    public void CashingHitInfo(HitInfo hit)
    {
        _cashedHitInfo = hit;
    }
}

#endregion

public class PlayerDeadState : PlayerState
{
    protected PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine) { }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}


