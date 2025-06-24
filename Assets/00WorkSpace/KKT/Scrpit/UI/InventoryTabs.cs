using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTabs : MonoBehaviour
{
    [Header("InventoryPanel")]
    public GameObject mapPanel;
    public GameObject itemPanel;
    public GameObject systemPanel;

    [Header("ItemPanel")]
    public GameObject itemMaterialPanel;
    public GameObject itemUsePanel;
    public GameObject itemWeaponPanel;
    public GameObject itemSkillPanel;
    public GameObject itemQuestPanel;

    public void ShowMap()
    {
        mapPanel.SetActive(true);
        itemPanel.SetActive(false);
        systemPanel.SetActive(false);
    }
    public void ShowItem()
    {
        mapPanel.SetActive(false);
        itemPanel.SetActive(true);
        systemPanel.SetActive(false);

        if (itemMaterialPanel != null) itemMaterialPanel.SetActive(true);
        if (itemUsePanel != null) itemUsePanel.SetActive(false);
        if (itemWeaponPanel != null) itemWeaponPanel.SetActive(false);
        if (itemSkillPanel != null) itemSkillPanel.SetActive(false);
        if (itemQuestPanel != null) itemQuestPanel.SetActive(false);
    }
    public void ShowSystem()
    {
        mapPanel.SetActive(false);
        itemPanel.SetActive(false);
        systemPanel.SetActive(true);
    }
}
