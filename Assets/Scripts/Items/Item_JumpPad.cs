using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpPadItem", menuName = "New Item/투척 아이템/점프패드")]
public class Item_JumpPad : Item_Throwing
{
    // 필요한 경우 추가 속성 정의
    [SerializeField] private float jumpForce = 15f; // 점프 힘 (더 높게 설정)

    // 기존 OnAttackEffect를 오버라이드
    public override void OnAttackEffect()
    {
        // 특별한 효과가 필요하면 여기에 구현
        Debug.Log("슈퍼 점프패드가 설치되었습니다!");
    }
}