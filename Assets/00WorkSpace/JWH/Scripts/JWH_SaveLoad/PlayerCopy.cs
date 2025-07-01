using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PlayerCopy
{
    [Header("저장 항목 선택")]
    public bool SavePosition = true;
    public bool SaveInventory = true;
    public bool SaveStats = true;
    //public bool SaveOtherStates = true;


    [HideInInspector] public Vector3 position;
    [HideInInspector] public int willPower, battery, maxBattery;
    [HideInInspector] public float moveSpeed, sprintSpeed, jumpForce;
    [HideInInspector] public int damage;
    [HideInInspector] public List<InventorySlotSaveData> inventorySlots = new List<InventorySlotSaveData>();

    public void Bring(PlayerStatus status)
    {
        if (SavePosition)
            position = status.transform.position;

        if (SaveStats)
        {
            willPower = status.CurrentWillPower.Value;
            battery = status.CurrentBattery.Value;
            maxBattery = status.MaxBattery.Value;
            moveSpeed = status.MoveSpeed;
            sprintSpeed = status.SprintSpeed;
            jumpForce = status.JumpForce;
            damage = status.Damage;
        }

        if (SaveInventory && status.inventory != null && status.inventory.model != null)
        {
            inventorySlots.Clear();
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                var slots = status.inventory.model.GetCurrentTabSlots(type);
                if (slots == null) continue;

                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot.item != null && slot.item.name != null)
                    {
                        inventorySlots.Add(new InventorySlotSaveData
                        {
                            itemName = slot.item.name,
                            count = slot.currentCount,
                            type = type,
                            slotIndex = i
                        });
                    }
                }
            }
        }
    }

    public void Give(PlayerStatus status)
    {
        if (SavePosition)
            status.transform.position = position;

        if (SaveStats)
        {
            status.CurrentWillPower.Value = willPower;
            status.CurrentBattery.Value = battery;
            status.MaxBattery.Value = maxBattery;
            status.MoveSpeed = moveSpeed;
            status.SprintSpeed = sprintSpeed;
            status.JumpForce = jumpForce;
            status.Damage = damage;
        }

        if (SaveInventory && status.inventory != null && status.inventory.model != null)
        {
            foreach (var data in inventorySlots)
            {
                var slots = status.inventory.model.GetCurrentTabSlots(data.type);
                if (slots == null || data.slotIndex >= slots.Count)
                {
                    Debug.LogWarning($"[Inventory Restore] 잘못된 슬롯 인덱스: type={data.type}, index={data.slotIndex}");
                    continue;
                }

                string path = $"ItemDatabase/{GetFolderForType(data.type)}/{data.itemName}";
                var item = Resources.Load<Item>(path);

                if (item != null)
                {
                    slots[data.slotIndex].item = item;
                    slots[data.slotIndex].currentCount = data.count;
                }
                else
                {
                    //Debug.LogWarning($"[Inventory Restore] 아이템 로드 실패: {path}");
                }
            }
        }
    }

    [Serializable]
    public class InventorySlotSaveData
    {
        public string itemName;
        public int count;
        public ItemType type;
        public int slotIndex;
    }


    private string GetFolderForType(ItemType type)
    {
        return type switch
        {
            ItemType.Ingredient => "01_Ingredient_Item",
            ItemType.Consumalbe => "02_Consumable_Item",
            ItemType.Equipment => "03_Equipment_Item",
            ItemType.Function => "04_Function_Item",
            ItemType.Quest => "05_Quest_Item",
            _ => "Unknown"
        };
    }
}
