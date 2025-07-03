using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/ȸ�� ������(������)/�Ϲ� ȸ��")]

public class Item_HealingHP : Item_Consumable
{
    [field: SerializeField] public int HealAmount { get; private set; }

    public void Hp_HealingRandomPart()
    {
        PlayerStatus ps = PlayerManager.Instance.instancePlayer.Status;

        // �������� ���� ���� ���� �ٵ� ��Ʈ ã��
        List<BodyPart> bodyParts = ps.GetBodyPartsList();
        BodyPart lowestBodyPart = null;
        int nowPartDamaged = 0;
        foreach (BodyPart bodyPart in bodyParts)
        {
            // ü���� ��� �ִ� ���¶��
            if (bodyPart.CurrentMaxHp- bodyPart.Hp > nowPartDamaged)
            {
                lowestBodyPart = bodyPart;
            }
        }
        if (lowestBodyPart != null)
        {
            lowestBodyPart.Repair(HealAmount);
            
        }
        else 
        {
            Debug.Log("��� ���� �̹� ü���� Ǯ�� �����ε�. �̶� ������ �Ҹ��ϰ� ������ �����?");
        }
    }
}
