using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : InteractableBase
{
    public Item item;
    public int count;

    public void InitInstance(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public override void Interact()
    {
        base.Interact();

        // 플레이어 인벤토리로 들어감
        pc.Status.inventory.AddItem(item, count);
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : 아이템 습득(E) 팝업 UI 활성화");
    }
}
