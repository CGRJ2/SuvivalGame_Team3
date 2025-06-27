using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Transform tf_parent;

    private int selectedSlot; //선택 퀵슬롯 (0~3) 4개

    [SerializeField]
    private GameObject go_SelectedImage; //선택 퀵슬롯 이미지

    private void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    private void Update()
    {
        //TryInPutNumber();
    }

    private void TryInPutNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
    }

    private void ChangeSlot(int _num)
    {

        SelectedSlot(_num);

        Execute();
    }

    private void SelectedSlot(int _num)
    {
        //선택슬롯
        selectedSlot = _num;
        //선택슬롯으로 이동
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;

    }

    private void Execute() // 맨손일때 바꿔주려 하는데 플레이어의 무기 장착이 먼저 구현되어야할 거 같음
    {
        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == ItemType.Equipment)
            { 
                //무기 일때 무기로 교체
            }
            else if (quickSlots[selectedSlot].item.itemType == ItemType.Used)
            {
                //아무것도 없을때 맨손 교체
            }
           
        }
        else 
        { 
            //아무것도 없을때 맨손 교체
        }
    }

}
