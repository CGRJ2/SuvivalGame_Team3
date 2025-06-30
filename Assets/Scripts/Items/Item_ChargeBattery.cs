using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/ȸ�� ������(���͸�)/������")]

public class Item_ChargeBattery : Item_Consumable
{
    [field: SerializeField] public int ChargeAmount { get; private set; }

    // ȸ�� ������ ��� ȿ�� (���͸�)
    public void Battery_Healing()
    {
        PlayerManager.Instance.instancePlayer.Status.ChargeBattery(ChargeAmount);
    }
}
