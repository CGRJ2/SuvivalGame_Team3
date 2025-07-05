using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Farming : InteractableBase, ISpawnable
{
    enum FarmingType
    {
        Drop_Immediately, Drop_AfterAnimation, AddToInventory_Immediately, AddToInventory_AfterAnimation
    }
    [SerializeField] DropTable dropTable;
    [SerializeField] Transform itemSpawnPoint;
    [SerializeField] FarmingType dropType;
    [SerializeField] GameObject destroyFxPrefab;

    public Action DeactiveAction { get; set; }
    public Transform OriginTransform { get; set; }

        

    // 드랍테이블 정보에 기입된 개수만큼 아이템 인스턴스 생성 반복
    public void DropItemInstances(DropInfo dropInfo)
    {
        for (int i = 0; i < dropInfo.dropCount; i++)
        {
            dropInfo.dropItem.SpawnItem(itemSpawnPoint);
        }
    }

    // 파밍 완료 후 비활성화 or 파괴
    public void DeactiveAfterFarmingDone()
    {
        // 일단 파괴 ===> 오브젝트풀 패턴으로 수정해야됨
        StartCoroutine(DestroyRouinte(1f));
    }

    private IEnumerator DestroyRouinte(float duration)
    {
        // 파괴 FX 실행(gameObject활성화)
        destroyFxPrefab.SetActive(true);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 파괴 FX 비활성화
        destroyFxPrefab.SetActive(false);
        Destroy(gameObject);
    }

    public override void OnDisableActions()
    {
        base.OnDisableActions();
        DeactiveAction?.Invoke();
    }

    public override void Interact()
    {
        base.Interact();
        DropInfo dropInfo = dropTable.GetDropItemInfo();

        switch (dropType)
        {
            case FarmingType.Drop_Immediately:
                // 개수만큼 아이템 인스턴스 드롭
                DropItemInstances(dropInfo);
                DeactiveAfterFarmingDone();
                break;

            case FarmingType.Drop_AfterAnimation:
                // 애니메이션 먼저 실행.
                // 애니메이션 진행 완료 후 실행
                // ==> 해당 파밍 애니메이션이 끝나는 시점에서 애니메이션 이벤트로 DropItemInstances(dropInfo)를 실행하는 걸로
                DropItemInstances(dropInfo);
                DeactiveAfterFarmingDone();
                break;

            case FarmingType.AddToInventory_Immediately:
                // 플레이어 인벤토리로 들어감
                pc.Status.inventory.AddItem(dropInfo.dropItem, dropInfo.dropCount);
                DeactiveAfterFarmingDone();
                break;

            case FarmingType.AddToInventory_AfterAnimation:
                // 애니메이션 먼저 실행.
                // 애니메이션 진행 완료 후 실행
                // 플레이어 인벤토리로 들어감
                pc.Status.inventory.AddItem(dropInfo.dropItem, dropInfo.dropCount);
                DeactiveAfterFarmingDone();
                break;
        }
    }

    public override void ShowInteractableUI()
    {
        base.ShowInteractableUI();

        // 파밍 오브젝트 종류별로 다르게 설정 필요
        UIManager.Instance.popUpUIGroup.interactableUI.tmp_InteractionMessage.text = $"채집: (E)";
    }
    
    
}
