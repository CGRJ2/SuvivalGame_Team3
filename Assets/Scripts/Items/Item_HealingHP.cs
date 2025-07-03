using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/회복 아이템(내구도)/일반 회복")]

public class Item_HealingHP : Item_Consumable
{
    [field: SerializeField] public int HealAmount { get; private set; }

    public void Hp_HealingRandomPart()
    {
        PlayerStatus ps = PlayerManager.Instance.instancePlayer.Status;

        // 데미지를 가장 많이 입은 바디 파트 찾기
        List<BodyPart> bodyParts = ps.GetBodyPartsList();
        BodyPart lowestBodyPart = null;
        int nowPartDamaged = 0;
        foreach (BodyPart bodyPart in bodyParts)
        {
            // 체력이 닳아 있는 상태라면
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
            Debug.Log("모든 부위 이미 체력이 풀인 상태인데. 이때 아이템 소모하게 만들까요 말까요?");
        }
    }
}
