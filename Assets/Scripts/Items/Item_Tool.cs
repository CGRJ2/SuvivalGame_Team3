using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/장비 아이템/도구 아이템")]
public class Item_Tool : Item, IEquipable
{

    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Equipment;
        maxCount = 1;
        itemName = this.name;
    }


    /*public void EquipToQuickSlot()
    {
        // 퀵슬롯 빈칸 판단 & 빈 퀵슬롯에 장착
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }*/

    public void OnAttackEffect()
    {
        // 커스텀 소비 효과가 있다면 실행
        if (ItemDatabase.AttackOnHandEffectDic.ContainsKey(itemName))
        {
            // 효과 실행
            ItemDatabase.AttackOnHandEffectDic[itemName].Invoke();
        }
    }
}
