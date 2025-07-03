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

    // ���� ���̽�ķ�� ������ ���� ���׷��̵� ���� ������Ʈ
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

    // ���׷��̵� ��� ������ �����͸� ��� ���Կ� ������Ʈ & ��� ����/�Ҹ��� �Ǻ�
    public bool IsRequiresSufficent()
    {
        bool isSufficent = true;

        // ���� ���׷��̵� ������ ������ǵ��� ��ȸ
        for (int i = 0; i < currentUpgradeCondition.requiredItems.Count; i++)
        {
            // ���� ��� ������ �����۰� �ʿ� ����
            Item requiredItem = currentUpgradeCondition.requiredItems[i].item;
            int requiredCount = currentUpgradeCondition.requiredItems[i].count;

            // �κ��丮 �� �ش� ���� ������ ���� ����
            InventoryPresenter playerInventory = PlayerManager.Instance.instancePlayer.Status.inventory;
            int owned = playerInventory.model.GetOwnedItemCount(requiredItem);
            Debug.Log($"{requiredItem.itemName}�� ���� �ν� ���� : {owned}");

            // ��� ���Կ� ���� ��� ���� ������ �־��ֱ�
            requiresSlotViews[i].requiredItemData = requiredItem;
            requiresSlotViews[i].UpdateSlotView(owned, requiredCount, textColor_requiresSufficient, textColor_requiresInsufficient);
            requiresSlotViews[i].gameObject.SetActive(true);

            // ���� �κ��丮�� ������ ����� ���� �����ϸ�
            if (owned < requiredCount)
            {
                // �ϴ� ���� �Ұ� ���·� �����, �ٸ� ��� ���Ե鵵 ��� ������ ������Ʈ�� ����� ��
                isSufficent = false;
            }
        }

        // ��� ������ ������ ��� ������ �������� ������ => ������ ���Ե��� ������ ����ְ� ��Ȱ��ȭ
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

    // �������� �ر��� �Ǿ� �ִ��� �Ǻ�
    public bool IsStageConditionUnlocked()
    {
        unlockStageRequireView.UpdateView(currentUpgradeCondition.needUnlockStage, textColor_requiresSufficient, textColor_requiresInsufficient);

        // �رݵǾ��ִ� ���¶��
        if (currentUpgradeCondition.needUnlockStage.IsUnlocked)
        {
            return true;
        }
        // �ر��� �ȵǾ��ִٸ�
        else
        {
            return false;
        }
    }
}
