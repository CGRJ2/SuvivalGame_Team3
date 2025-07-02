using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftInventory : MonoBehaviour
{
    public Transform slotParent;
    public GameObject slotPrefab;
    public TextMeshProUGUI pageText;

    public Button prevButton;
    public Button nextButton;

    public int slotPerPage = 16;

    private List<ItemData> allItems = new List<ItemData>();
    private int currentPage = 1;
    private int totalPage = 4;

    public class ItemData
    {
        public string itemName;
        public ItemData(string name)
        {
            itemName = name;
        }
    }
    private void Start()
    {
        for (int i = 0; i < 36; i++)
        {
            allItems.Add(new ItemData("아이템 이름"));
        }
        UpdatePage();
    }

    void UpdatePage()
    {
        foreach(Transform t in slotParent)
        {
            Destroy(t.gameObject);
        }

        totalPage=Mathf.CeilToInt((float)allItems.Count / slotPerPage);

        int startIndex = (currentPage - 1) * slotPerPage;
        int endIndex=Mathf.Min(startIndex+slotPerPage,allItems.Count);

        for(int i = startIndex; i < endIndex; i++)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            slot.transform.Find("Text_ItemName").GetComponent<TextMeshProUGUI>().text=allItems[i].itemName;
        }

        pageText.text = $"{currentPage}/{totalPage}";
        prevButton.interactable = currentPage > 1;
        nextButton.interactable = currentPage < totalPage;
    }

    public void OnPrevPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdatePage();
        }
    }
    public void OnNextPage()
    {
        if (currentPage < totalPage)
        {
            currentPage++;
            UpdatePage();
        }
    }
}
