using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable_StageLocker : InteractableBase
{
    public GameObject blockWall;
    public GameObject afterUnlock;
    
    [SerializeField] StageData chainedStageData;

    public bool IsCanUlock()
    {
        bool isSufficent = true;

        for (int i = 0; i < chainedStageData.unlockCondition.needItemList.Count; i++)
        {
            // 현재 재료 조건의 아이템과 필요 수량
            Item requiredItem = chainedStageData.unlockCondition.needItemList[i].item;
            int requiredCount = chainedStageData.unlockCondition.needItemList[i].count;

            // 인벤토리 내 해당 조건 체크 (아이템 종류 & 개수)
            InventoryPresenter playerInventory = PlayerManager.Instance.instancePlayer.Status.inventory;
            int owned = playerInventory.model.GetOwnedItemCount(requiredItem);
            Debug.Log($"{requiredItem.itemName}의 현재 인식 개수 : {owned}");

            // 현재 인벤토리에 보유한 재료의 수가 부족하면
            if (owned < requiredCount)
            {
                // 상호작용 불가
                isSufficent = false;
            }
        }
        return isSufficent;
    }

    public override void Interact()
    {
        base.Interact();

        // 손에 들고 있는 아이템이 요구 아이템과 같을 경우 (열쇠를 손에 들고 상호작용하려면 이걸 사용하자)
        /*if (pc.Status.onHandItem == itemForUnlock)
        {
            Debug.Log("잠금 해제");
            gameObject.SetActive(false);
        }*/

        if (IsCanUlock())
        {
            Debug.Log($"[{chainedStageData.StageName}] 잠금 해제");
            chainedStageData.UlockStage();
            blockWall.SetActive(false);
            afterUnlock.SetActive(true);
        }

        
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();

        // 손에 들고 있는 아이템이 요구 아이템과 같을 경우 => 상호작용 가능
        /*if (pc.Status.onHandItem == itemForUnlock)
            Debug.Log($"열쇠 사용(E)");*/

        if (IsCanUlock())
        {
            if (chainedStageData.unlockCondition.needItemList.Count > 0)
            {
                UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = 
                    $"{chainedStageData.unlockCondition.needItemList[0].item.itemName} 사용: (E)";
            }
            else
            {
                Debug.LogError("언락 조건이 설정되지 않은 잠금장치 사용");
            }
        }
        else
        {
            UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"굳게 잠겨있다...";
        }
    }
}
