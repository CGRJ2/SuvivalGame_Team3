using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTabs : MonoBehaviour
{
    [Header("ItemPanel")]
    public GameObject materialItemPanel;
    public GameObject useItemPanel;
    public GameObject weaponItemPanel;
    public GameObject functionItemPanel;
    public GameObject questItemPanel;

    public void ShowMaterial()
    {
        materialItemPanel.SetActive(true);
        useItemPanel.SetActive(false);
        weaponItemPanel.SetActive(false);
        functionItemPanel.SetActive(false);
        questItemPanel.SetActive(false);
    }
    public void ShowUse()
    {
        materialItemPanel.SetActive(false);
        useItemPanel.SetActive(true);
        weaponItemPanel.SetActive(false);
        functionItemPanel.SetActive(false);
        questItemPanel.SetActive(false);
    }
    public void ShowWeapon()
    {
        materialItemPanel.SetActive(false);
        useItemPanel.SetActive(false);
        weaponItemPanel.SetActive(true);
        functionItemPanel.SetActive(false);
        questItemPanel.SetActive(false);
    }
    public void ShowFunction()
    {
        materialItemPanel.SetActive(false);
        useItemPanel.SetActive(false);
        weaponItemPanel.SetActive(false);
        functionItemPanel.SetActive(true);
        questItemPanel.SetActive(false);
    }
    public void ShowQuest()
    {
        materialItemPanel.SetActive(false);
        useItemPanel.SetActive(false);
        weaponItemPanel.SetActive(false);
        functionItemPanel.SetActive(false);
        questItemPanel.SetActive(true);
    }
}
