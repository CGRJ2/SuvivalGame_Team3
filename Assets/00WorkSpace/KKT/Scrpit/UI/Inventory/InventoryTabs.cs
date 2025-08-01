using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTabs : MonoBehaviour
{
    [Header("InventoryPanel")]
    public GameObject mapPanel;
    public GameObject itemPanel;
    public GameObject systemPanel;

    [Header("SystemPanel")]
    public GameObject HomeWarning;

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
    }
    public void ShowSystem()
    {
        mapPanel.SetActive(false);
        itemPanel.SetActive(false);
        systemPanel.SetActive(true);
    }
    public void ShowHomeWarning()
    {
        HomeWarning.SetActive(true);
    }
    public void CloseHomeWarning()
    {
        HomeWarning.SetActive(false);
    }

    public void CloseInventory()
    {
        UIController.Instance.ShowInventory(false);
    }
}
