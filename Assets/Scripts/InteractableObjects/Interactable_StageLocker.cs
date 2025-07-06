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
            // ���� ��� ������ �����۰� �ʿ� ����
            Item requiredItem = chainedStageData.unlockCondition.needItemList[i].item;
            int requiredCount = chainedStageData.unlockCondition.needItemList[i].count;

            // �κ��丮 �� �ش� ���� üũ (������ ���� & ����)
            InventoryPresenter playerInventory = PlayerManager.Instance.instancePlayer.Status.inventory;
            int owned = playerInventory.model.GetOwnedItemCount(requiredItem);
            Debug.Log($"{requiredItem.itemName}�� ���� �ν� ���� : {owned}");

            // ���� �κ��丮�� ������ ����� ���� �����ϸ�
            if (owned < requiredCount)
            {
                // ��ȣ�ۿ� �Ұ�
                isSufficent = false;
            }
        }
        return isSufficent;
    }

    public override void Interact()
    {
        base.Interact();

        // �տ� ��� �ִ� �������� �䱸 �����۰� ���� ��� (���踦 �տ� ��� ��ȣ�ۿ��Ϸ��� �̰� �������)
        /*if (pc.Status.onHandItem == itemForUnlock)
        {
            Debug.Log("��� ����");
            gameObject.SetActive(false);
        }*/

        if (IsCanUlock())
        {
            Debug.Log($"[{chainedStageData.StageName}] ��� ����");
            chainedStageData.UlockStage();
            blockWall.SetActive(false);
            afterUnlock.SetActive(true);
        }

        
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();

        // �տ� ��� �ִ� �������� �䱸 �����۰� ���� ��� => ��ȣ�ۿ� ����
        /*if (pc.Status.onHandItem == itemForUnlock)
            Debug.Log($"���� ���(E)");*/

        if (IsCanUlock())
        {
            if (chainedStageData.unlockCondition.needItemList.Count > 0)
            {
                UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = 
                    $"{chainedStageData.unlockCondition.needItemList[0].item.itemName} ���: (E)";
            }
            else
            {
                Debug.LogError("��� ������ �������� ���� �����ġ ���");
            }
        }
        else
        {
            UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"���� ����ִ�...";
        }
    }
}
