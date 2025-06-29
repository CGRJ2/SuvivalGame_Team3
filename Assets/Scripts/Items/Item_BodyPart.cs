using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/ȸ�� ������(������)/��ü ��ǰ")]
public class Item_BodyPart : Item_Consumable
{
    [field: SerializeField] public BodyPartTypes targetPart { get; private set; }

    // ���͸� ��ü ȿ��
    public void Hp_Init()
    {
        PlayerStatus ps = PlayerManager.Instance.instancePlayer.Status;
        ps.GetPart(targetPart).Init();
    }
}
