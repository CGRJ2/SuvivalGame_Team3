using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeRequires : MonoBehaviour
{
    [SerializeField] Transform requiresSlotsParent;
    RequiresSlotView[] requiresSlotViews;
    [SerializeField] UnlockStageRequireView unlockStageRequireView;
    [SerializeField] Color textColor_requiresSufficient;
    [SerializeField] Color textColor_requiresInsufficient;

    public BaseCampUpgradeCondition currentUpgradeCondition;


    public void Init()
    {
        requiresSlotViews = requiresSlotsParent.GetComponentsInChildren<RequiresSlotView>();
        BaseCampManager.Instance.baseCampData.CurrentCampLevel.Subscribe(SetCurrentUpgradeCondition);
        SetCurrentUpgradeCondition(BaseCampManager.Instance.baseCampData.CurrentCampLevel.Value);
    }

    private void OnDestroy()
    {
        if (BaseCampManager.Instance != null)
        BaseCampManager.Instance.baseCampData.CurrentCampLevel.Unsubscribe(SetCurrentUpgradeCondition);
    }

    // 현재 베이스캠프 레벨에 따라 업그레이드 조건 업데이트
    public void SetCurrentUpgradeCondition(int currentLevel)
    {
        BaseCampUpgradeCondition[] upgradeConditions = BaseCampManager.Instance.UpgradeConditions;

        foreach (BaseCampUpgradeCondition nowCondition in upgradeConditions)
        {
            if (nowCondition.currentLevel == currentLevel)
            {
                currentUpgradeCondition = nowCondition;
                return;
            }
        }
    }

    // 업그레이드 재료 아이템 데이터를 재료 슬롯에 업데이트 & 재료 만족/불만족 판별
    public bool IsRequiresSufficent()
    {
        bool isSufficent = true;

        // 현재 업그레이드 조건의 재료조건들을 순회
        for (int i = 0; i < currentUpgradeCondition.requiredItems.Count; i++)
        {
            // 현재 재료 조건의 아이템과 필요 수량
            Item requiredItem = currentUpgradeCondition.requiredItems[i].item;
            int requiredCount = currentUpgradeCondition.requiredItems[i].count;

            // 인벤토리 내 해당 조건 아이템 보유 수량
            InventoryPresenter playerInventory = PlayerManager.Instance.instancePlayer.Status.inventory;
            int owned = playerInventory.model.GetOwnedItemCount(requiredItem);
            Debug.Log($"{requiredItem.itemName}의 현재 인식 개수 : {owned}");

            // 재료 슬롯에 현재 재료 조건 데이터 넣어주기
            requiresSlotViews[i].requiredItemData = requiredItem;
            requiresSlotViews[i].UpdateSlotView(owned, requiredCount, textColor_requiresSufficient, textColor_requiresInsufficient);
            requiresSlotViews[i].gameObject.SetActive(true);

            // 현재 인벤토리에 보유한 재료의 수가 부족하면
            if (owned < requiredCount)
            {
                // 일단 제작 불가 상태로 만들고, 다른 재료 슬롯들도 재료 데이터 업데이트는 해줘야 함
                isSufficent = false;
            }
        }

        // 재료 조건의 개수가 재료 슬롯의 개수보다 적으면 => 나머지 슬롯들의 데이터 비워주고 비활성화
        if (currentUpgradeCondition.requiredItems.Count < requiresSlotViews.Length)
        {
            for (int i = currentUpgradeCondition.requiredItems.Count; i < requiresSlotViews.Length; i++)
            {
                requiresSlotViews[i].ClearSlotView();
                requiresSlotViews[i].gameObject.SetActive(false);
            }
        }

        return isSufficent;
    }

    // 스테이지 해금이 되어 있는지 판별
    public bool IsStageConditionUnlocked()
    {
        unlockStageRequireView.UpdateView(currentUpgradeCondition.needUnlockStage, textColor_requiresSufficient, textColor_requiresInsufficient);

        // 해금되어있는 상태라면
        if (currentUpgradeCondition.needUnlockStage.IsUnlocked)
        {
            return true;
        }
        // 해금이 안되어있다면
        else
        {
            return false;
        }
    }
}
