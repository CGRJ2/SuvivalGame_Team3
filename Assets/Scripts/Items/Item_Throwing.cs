using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/투척 아이템")]

public class Item_Throwing : Item_Consumable, IEquipable
{
    public void ActivateEffectOnAttack()
    {

    }


    public void EquipToQuickSlot()
    {
        // 퀵슬롯 빈칸 판단 & 빈 퀵슬롯에 장착
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }

    public void OnAttackEffect()
    {
        // 던지기 기능을 위해 임시로 추가한 내용
        Debug.Log($"{itemName} 아이템을 던졌습니다!");
        // 여기에 던지기 효과 관련 코드 추가 (소리, 파티클 등)
    }
}
