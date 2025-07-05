using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/��� ������/���� ������")]
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
        // ������ ��ĭ �Ǵ� & �� �����Կ� ����
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }*/

    public void OnAttackEffect()
    {
        // Ŀ���� �Һ� ȿ���� �ִٸ� ����
        if (ItemDatabase.AttackOnHandEffectDic.ContainsKey(itemName))
        {
            // ȿ�� ����
            ItemDatabase.AttackOnHandEffectDic[itemName].Invoke();
        }
    }
}
