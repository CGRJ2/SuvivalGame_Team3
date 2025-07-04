using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public ItemData itemData;
    private InventoryTooltip tooltip;

    public void SetData(ItemData data, InventoryTooltip tooltipPanel)
    {
        itemData = data;
        tooltip = tooltipPanel;
        if(iconImage != null)
        {
            iconImage.sprite = data.icon;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && tooltip != null)
        {
            tooltip.Show(itemData.itemName, itemData.itemDesc, itemData.icon);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.Hide();
        }
    }
}
