using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/회복 아이템(배터리)/충전기")]

public class Item_ChargeBattery : Item_Consumable
{
    [field: SerializeField] public int ChargeAmount { get; private set; }

    // 회복 아이템 사용 효과 (배터리)
    public void Battery_Healing()
    {
        PlayerManager.Instance.instancePlayer.Status.ChargeBattery(ChargeAmount);
    }
}
