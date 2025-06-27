using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp_Interactable_Save : InteractableBase
{
    //  상호작용은 UI를 여는 용도일 뿐, 아랫 내용은 이후 UI에서 판단/실행하도록 분리한다
    public override void Interact()
    {
        Debug.Log($"{gameObject.name} : 상호작용 실행");
    }

    public override void SetInteractableEnable()
    {
        Debug.Log($"{gameObject.name} : 상호작용 범위 진입");
    }
}
