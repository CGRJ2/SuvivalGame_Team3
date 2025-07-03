using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Upgrade : InteractableBase
{
    public override void Interact()
    {
        Debug.Log($"업그레이드 UI 활성화");
        UIManager.Instance.upgradeGroup.OpenPanel_Base();

    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : 상호작용 범위 진입");
    }
}
