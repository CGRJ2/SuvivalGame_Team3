using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text Text_ItemName;
    [SerializeField]
    private Text Text_ItemDesc;
    [SerializeField]
    private Text Text_ItemHowToUesd;

    private void Start()
    {
        UIManager.Instance.inventoryUI.tooltip = this;
    }

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height, 0f);

        go_Base.transform.position = _pos;
        Text_ItemName.text = _item.itemName;
        Text_ItemDesc.text = _item.description;

        if (_item.itemType == ItemType.Equipment)
        {
            Text_ItemHowToUesd.text = "우클릭 - 장착";
        }
        else if (_item.itemType == ItemType.Used)
        {
            Text_ItemHowToUesd.text = "우클릭 - 사용하기";
        }
        else
        {
            Text_ItemHowToUesd.text = "";
        }
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }

}
