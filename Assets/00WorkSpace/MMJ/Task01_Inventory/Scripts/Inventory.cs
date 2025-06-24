using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_inventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;
    [SerializeField]
    private GameObject go_QuickSlotParent;

    [SerializeField]
    private GameObject go_Base; //Base_Outer 참조
    

    //인벤토리 슬롯들
    private Slot[] slots;
    //퀵슬롯들
    private Slot[] quickSlots;
    private bool isNotPut;


    private void Start()
    {
        
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        go_inventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_inventoryBase.SetActive(false);
        go_Base.SetActive(false); //툴팁을 띄운채로 인벤토리를 닫으면 툴팁이 유지되는 버그가 있어서 수정한 내용 
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        PutSlot(quickSlots, _item, _count);
        if (isNotPut)
        {
            PutSlot(slots, _item, _count);
        }

        if (isNotPut)
            Debug.Log("인벤토리가 꽉찼옹~");

    }

    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null && _slots[i].item != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null && _slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }

}
