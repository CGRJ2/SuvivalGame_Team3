using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Interactable_RegionLock : InteractableBase
{
    public Item itemForUnlock;
    public GameObject blockWall;

    public override void Interact()
    {
        base.Interact();

        // 손에 들고 있는 아이템이 요구 아이템과 같을 경우
        if (pc.Status.onHandItem == itemForUnlock)
        {
            Debug.Log("잠금 해제");
            gameObject.SetActive(false);
        }
        
    }

    public override void SetInteractableEnable()
    {
        base.SetInteractableEnable();

        // 손에 들고 있는 아이템이 요구 아이템과 같을 경우 => 상호작용 가능
        if (pc.Status.onHandItem == itemForUnlock)
            Debug.Log($"열쇠 사용(E)");
    }
}
