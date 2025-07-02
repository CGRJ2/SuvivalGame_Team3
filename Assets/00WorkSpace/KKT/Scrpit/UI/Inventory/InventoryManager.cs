using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform slotParent;         // 슬롯 부모 (Panel 등)
    public GameObject slotPrefab;        // 슬롯 프리팹
    public InventoryTooltip tooltipPanel;    // 툴팁 패널

    public int slotPerPage = 16;
    private int currentPage = 1;
    private int totalPage = 1;
    private List<ItemData> allItems = new List<ItemData>();

    void Start()
    {
        // 예시 데이터 (실제 구현 시 외부에서 불러오기)
        allItems.Add(new ItemData("손전등", "누가봐도 수상한 손전등", Resources.Load<Sprite>("Icons/Flashlight")));
        allItems.Add(new ItemData("붕대", "응급처치용 붕대", Resources.Load<Sprite>("Icons/Bandage")));
        // ...원하는 만큼 추가

        UpdatePage();
    }

    void UpdatePage()
    {
        // 슬롯 비우기
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        int startIndex = (currentPage - 1) * slotPerPage;
        int endIndex = Mathf.Min(startIndex + slotPerPage, allItems.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            var slotGO = Instantiate(slotPrefab, slotParent);
            var slotUI = slotGO.GetComponent<InventorySlot>();
            slotUI.SetData(allItems[i], tooltipPanel); // 혹은 적절한 방식으로 데이터 전달
        }
    }
}