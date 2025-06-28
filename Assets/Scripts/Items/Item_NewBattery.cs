using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/회복 아이템(배터리)/배터리")]

public class Item_NewBattery : Item_Consumable
{
    // 배터리 교체 효과
    public void Battery_Init()
    {
        PlayerManager.Instance.instancePlayer.Status.InitBattery();
    }
}
