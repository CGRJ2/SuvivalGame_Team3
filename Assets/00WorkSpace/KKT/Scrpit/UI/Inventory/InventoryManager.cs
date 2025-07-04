using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform slotParent;         // ���� �θ� (Panel ��)
    public GameObject slotPrefab;        // ���� ������
    public InventoryTooltip tooltipPanel;    // ���� �г�

    public int slotPerPage = 16;
    private int currentPage = 1;
    private int totalPage = 1;
    private List<ItemData> allItems = new List<ItemData>();

    void Start()
    {
        // ���� ������ (���� ���� �� �ܺο��� �ҷ�����)
        allItems.Add(new ItemData("������", "�������� ������ ������", Resources.Load<Sprite>("Icons/Flashlight")));
        allItems.Add(new ItemData("�ش�", "����óġ�� �ش�", Resources.Load<Sprite>("Icons/Bandage")));
        // ...���ϴ� ��ŭ �߰�

        UpdatePage();
    }

    void UpdatePage()
    {
        // ���� ����
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
            slotUI.SetData(allItems[i], tooltipPanel); // Ȥ�� ������ ������� ������ ����
        }
    }
}