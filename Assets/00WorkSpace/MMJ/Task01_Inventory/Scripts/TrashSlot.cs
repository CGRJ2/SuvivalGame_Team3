using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            // ���� ������ ó��
            Debug.Log(DragSlot.instance.dragSlot.item.itemName + " �������� ���Ƚ��ϴ�.");

            // �巡���ϴ� ���� ����
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
