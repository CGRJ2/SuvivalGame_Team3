using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.XR;

public class InventoryTooltip : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public Image itemImage;

    public Vector2 offset = new Vector2(20, -20);

    
    public void Show(string _itemName, string _itemDesc, Sprite _itemSprite)
    {
        itemName.text = _itemName;
        itemDesc.text = _itemDesc;
        itemImage.sprite = _itemSprite;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, null, out pos);
        transform.position = pos + offset;
    }
}
