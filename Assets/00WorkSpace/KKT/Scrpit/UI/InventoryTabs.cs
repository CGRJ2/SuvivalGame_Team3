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
    public GameObject materialItemPanel;
    public GameObject useItemPanel;
    public GameObject weaponItemPanel;
    public GameObject functionItemPanel;
    public GameObject questItemPanel;
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

        if (materialItemPanel != null) materialItemPanel.SetActive(true);
        if (useItemPanel != null) useItemPanel.SetActive(false);
        if (weaponItemPanel != null) weaponItemPanel.SetActive(false);
        if (functionItemPanel != null) functionItemPanel.SetActive(false);
        if (questItemPanel != null) questItemPanel.SetActive(false);
    }
    public void ShowSystem()
    {
        mapPanel.SetActive(false);
        itemPanel.SetActive(false);
        systemPanel.SetActive(true);
    }
}
