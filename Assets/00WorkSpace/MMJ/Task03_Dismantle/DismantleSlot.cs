using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DismantleSlot : MonoBehaviour
{
    public Item item;
    public Image icon;
    public Text nameText;

    public void SetItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.itemImage;
        icon.enabled = true;
        nameText.text = newItem.itemName;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        nameText.text = "";
    }
}
