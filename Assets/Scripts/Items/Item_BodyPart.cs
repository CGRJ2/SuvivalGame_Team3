using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/회복 아이템(내구도)/신체 부품")]
public class Item_BodyPart : Item_Consumable
{
    [field: SerializeField] public BodyPartType targetPart { get; private set; }

    // 배터리 교체 효과
    public void Hp_Init()
    {
        PlayerModel ps = PlayerManager.Instance.instancePlayer.Model;
        ps.GetBodyPartsDic()[targetPart].Init();
    }
}
