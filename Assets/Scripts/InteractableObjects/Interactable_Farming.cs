using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Interactable_Farming : InteractableBase
{
    enum FarmingType
    {
        Drop_Immediately, Drop_AfterAnimation, AddToInventory_Immediately, AddToInventory_AfterAnimation
    }
    [SerializeField] Item dropItem;
    [SerializeField] FarmingType dropType;

   
    public override void Interact()
    {
        base.Interact();

        switch (dropType)
        {
            case FarmingType.Drop_Immediately:

                // 아이템 인스턴스 드롭
                dropItem.SpawnItem(transform);

                break;
            case FarmingType.Drop_AfterAnimation:

                // 애니메이션 진행 완료 후 실행
                dropItem.SpawnItem(transform);

                break;
            case FarmingType.AddToInventory_Immediately:

                // 플레이어 인벤토리로 들어감
                Debug.Log(pc);
                pc.Status.inventory.AddItem(dropItem);

                break;
            case FarmingType.AddToInventory_AfterAnimation:

                // 플레이어 인벤토리로 들어감
                pc.Status.inventory.AddItem(dropItem);
                break;
        }
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        Debug.Log($"{gameObject.name} : 채집(E) 팝업 UI 활성화");
    }
}
