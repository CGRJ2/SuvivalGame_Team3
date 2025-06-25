using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlotView : MonoBehaviour
{
    static public DragSlotView instance;

    public SlotView dragSlot;

    //아이템 이미지
    [SerializeField]
    private Image imageItem;

    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void DropClearImage()
    {
        imageItem.sprite = null;
        SetColor(0);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
