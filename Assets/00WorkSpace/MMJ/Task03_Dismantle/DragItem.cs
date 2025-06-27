using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragItem : MonoBehaviour
{
    public static DragItem instance;

    public Image dragImage;
    public Item draggingItem;

    private void Awake()
    {
        instance = this;
        dragImage.raycastTarget = false;  // 드래그 중 클릭 방지
        Hide();
    }

    public void Show(Item item, Sprite icon)
    {
        draggingItem = item;
        dragImage.sprite = icon;
        dragImage.enabled = true;
    }

    public void Hide()
    {
        draggingItem = null;
        dragImage.enabled = false;
    }

    void Update()
    {
        if (dragImage.enabled)
        {
            dragImage.transform.position = Input.mousePosition;
        }
    }
}
