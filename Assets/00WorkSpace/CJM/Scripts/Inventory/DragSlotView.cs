using UnityEngine;
using UnityEngine.UI;

public class DragSlotView : MonoBehaviour
{
    public SlotView slot;

    //아이템 이미지
    [SerializeField]
    private Image itemSprite;

    private void Start() => Init();



    public void Init()
    {
        UIManager.Instance.inventoryUI.dragSlotInstance = this;
    }

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
