using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class RequiresSlotView : MonoBehaviour
{
    public Item requiredItemData;

    public Image itemImage;
    public TMP_Text tmp_ItemName;
    public TMP_Text tmp_ItemCountRQbyOwned;

    public void UpdateSlotView(int owndCount, int requiredCount, Color ActiveColor, Color DeactiveColor)
    {
        itemImage.sprite = requiredItemData.imageSprite;
        tmp_ItemName.text = requiredItemData.itemName;

        if (owndCount < requiredCount)
        {
            tmp_ItemCountRQbyOwned.color = DeactiveColor;
        }
        else
        {
            tmp_ItemCountRQbyOwned.color = ActiveColor;
        }
        tmp_ItemCountRQbyOwned.text = $"{owndCount}/{requiredCount}";
    }

    public void ClearSlotView()
    {
        requiredItemData = null;
        itemImage.sprite = default;
        tmp_ItemName.text = default;
        tmp_ItemCountRQbyOwned.color = default;
        tmp_ItemCountRQbyOwned.text = "00/00";
    }

    
}
