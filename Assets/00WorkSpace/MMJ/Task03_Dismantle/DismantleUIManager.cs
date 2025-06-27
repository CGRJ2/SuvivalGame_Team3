using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DismantleUIManager : MonoBehaviour
{
    [Header("해체 슬롯 10개")]
    public Slot[] dismantleSlots;

    [Header("인벤토리 참조")]
    public Inventory inventory;

    [Header("해체 버튼")]
    public Button dismantleButton;

    [Header("보상 아이템")]
    public Item cotton;
    public Item batteryPart;
    public Item dust;
    public Item plastic;
    public Item glue;
    public Item circuitBoard;

    [Header("레어 아이템 확률")]
    [Range(0f, 1f)]
    public float glueChance = 0.2f;
    [Range(0f, 1f)]
    public float circuitBoardChance = 0.1f;

    private void Start()
    {
        dismantleButton.onClick.AddListener(OnClickDismantle);
    }

    private void OnClickDismantle()
    {
        foreach (var slot in dismantleSlots)
        {
            if (slot.item != null && slot.itemCount > 0)
            {
                int stackCount = slot.itemCount;

                for (int i = 0; i < stackCount; i++)
                {
                    // 1. 아이템 1개씩 소모
                    inventory.RemoveItem(slot.item, 1);

                    // 2. 일반 아이템 지급 (1~5 랜덤)
                    inventory.AcquireItem(cotton, Random.Range(1, 6));
                    inventory.AcquireItem(batteryPart, Random.Range(1, 6));
                    inventory.AcquireItem(dust, Random.Range(1, 6));
                    inventory.AcquireItem(plastic, Random.Range(1, 6));

                    // 3. 레어 아이템 확률
                    if (Random.value < glueChance)
                        inventory.AcquireItem(glue, 1);
                    if (Random.value < circuitBoardChance)
                        inventory.AcquireItem(circuitBoard, 1);
                }

                // 4. 슬롯 비우기
                slot.ClearSlot();
            }
        }
    }
}