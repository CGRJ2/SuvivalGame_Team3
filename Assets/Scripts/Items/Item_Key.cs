using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/����Ʈ ������/����")]
public class Item_Key : Item, IEquipable
{
    public void EquipToQuickSlot()
    {
        // ������ ���
        PlayerManager.Instance.instancePlayer.Status.onHandItem = this;
    }

    public void OnAttackEffect()
    {
        // �÷��̾ �ش� �ڹ��� ������Ʈ�� ��ȣ�ۿ� ���� �ȿ� �����Ѵٸ�
        Debug.Log("�������� �Ŵ������� �ش� �������� ���");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Quest;
        maxCount = 1;
        itemName = this.name;
        imageSprite = Resources.Load<Sprite>("Sprites/ItemIcons/Sprite_Quest");
    }
}
