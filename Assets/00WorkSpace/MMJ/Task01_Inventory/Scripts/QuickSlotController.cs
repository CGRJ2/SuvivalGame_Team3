using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Transform tf_parent;

    private int selectedSlot; //���� ������ (0~3) 4��

    [SerializeField]
    private GameObject go_SelectedImage; //���� ������ �̹���

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
        //���ý���
        selectedSlot = _num;
        //���ý������� �̵�
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;

    }

    private void Execute() // �Ǽ��϶� �ٲ��ַ� �ϴµ� �÷��̾��� ���� ������ ���� �����Ǿ���� �� ����
    {
        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == ItemType.Equipment)
            { 
                //���� �϶� ����� ��ü
            }
            else if (quickSlots[selectedSlot].item.itemType == ItemType.Used)
            {
                //�ƹ��͵� ������ �Ǽ� ��ü
            }
           
        }
        else 
        { 
            //�ƹ��͵� ������ �Ǽ� ��ü
        }
    }

}
