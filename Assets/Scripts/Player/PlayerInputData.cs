using UnityEngine;

/// <summary>
/// 장치/키보드 종류와 무관하게
/// *플레이어가 지금 어떤 입력 상태인지* 만 담아두는 데이터
/// </summary>
public sealed class PlayerInputData
{
    // **** 이동 / 카메라 ****
    /// <summary>WASD, 왼쪽 스틱 등 → 이동 벡터</summary>
    public Vector2 Move { get; set; }

    /// <summary>마우스/오른쪽 스틱 → 카메라 회전</summary>
    public Vector2 Rotate { get; set; }

    /// <summary>휠/트리거</summary>
    public float ZoomDelta { get; set; }
    

    // **** 기본 액션 ****
    // Jump
    public bool JumpPressed { get; set; }   // 이번 프레임에 막 눌림
    public bool JumpHeld { get; set; }   // 누르고 있는 동안 true
    public bool JumpReleased { get; set; }   // 이번 프레임에 막 뗌

    // Attack
    public bool AttackPressed { get; set; }
    public bool AttackHeld { get; set; }
    public bool AttackReleased { get; set; }

    // 누르고 있는 동안 동작
    // 토글/홀드 둘 다 쓸 수 있게 수정 필요
    public bool AimingHeld { get; set; }
    public bool SprintHeld { get; set; }
    public bool CrouchHeld { get; set; }

    // 한 번 눌림
    public bool RollPressed { get; set; }
    public bool InteractionPressed { get; set; }
    public bool InventoryPressed { get; set; }

    // 퀵슬롯: 1~4 중 방금 눌린 슬롯 인덱스 (-1이면 없음)
    public int QuickSlotIndexPressed { get; set; } = -1;

    // **** 프레임마다 초기화할 것들 ****
    /// 프레임에 눌림/떼짐 플래그 리셋
    public void ClearFrameFlags()
    {
        JumpPressed         = false;
        JumpReleased        = false;

        AttackPressed       = false;
        AttackReleased      = false;

        RollPressed         = false;

        InteractionPressed  = false;

        InventoryPressed    = false;

        QuickSlotIndexPressed = -1;
    }
}