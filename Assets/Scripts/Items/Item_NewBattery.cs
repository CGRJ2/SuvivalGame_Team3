using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/ȸ�� ������(���͸�)/���͸�")]

public class Item_NewBattery : Item_Consumable
{
    // ���͸� ��ü ȿ��
    public void Battery_Init()
    {
        PlayerManager.Instance.instancePlayer.Status.InitBattery();
    }
}
