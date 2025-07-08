using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_GrandFather : InteractableBase
{
    bool isUsed;

    [Header("할아버지 상호작용 회복량")]
    [SerializeField] float recoverAmount;

    [Header("상호작용 메세지(회복가능)")]
    [SerializeField] string interactMessage_Active;

    [Header("상호작용 메세지(회복불가)")]
    [SerializeField] string interactMessage_Deactive;

    [Header("회복 완료 시 메세지")]
    [SerializeField] string recoverMessage;
    

    public void Start()
    {
        DailyManager.Instance.currentTimeData.TZ_State.Subscribe(CheckMorningInit);
    }

    public override void OnDisableActions()
    {
        base.OnDisableActions();
        DailyManager.Instance.currentTimeData.TZ_State.Unsubscribe(CheckMorningInit);

    } 

    public void CheckMorningInit(TimeZoneState timeZoneState)
    {
        if (timeZoneState == TimeZoneState.Morning) isUsed = false;
    }


    public override void Interact()
    {
        
        if (isUsed) return;
        isUsed = true;
        PlayerManager.Instance.instancePlayer.Status.CurrentWillPower.Value += recoverAmount;
        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = recoverMessage;
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();
        if (isUsed)
        {
            UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = interactMessage_Deactive;
        }
        else
        {
            UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = interactMessage_Active;
        }
    }
}
