using UnityEngine;
using UnityEngine.UI;

public class DragSlotView : MonoBehaviour
{
    public SlotView slotView;

    //아이템 이미지
    [SerializeField]
    private Image itemSprite;

    public void DragSetImage(Image _itemImage)
    {
        itemSprite.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void DropClearImage()
    {
        itemSprite.sprite = null;
        SetColor(0);
    }

    public void SetColor(float _alpha)
    {
        Color color = itemSprite.color;
        color.a = _alpha;
        itemSprite.color = color;
    }
}
