using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템의 이름(키값)
    public string[] part; //부위
    public int[] num; //수치

}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    //필요한 컴포넌트 -> 플레이어의 체력과 정신력 수치, 배터리등에 대해서 정보를 받아와야 함
    //todo private StatusController thePlayerStatus;
    [SerializeField]
    private SlotToolTip theSlotToolTip;

    private const string HP = "HP", SP = "SP", BATTERY = "BATTERY", WILL = "WILL";

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }

    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Used) //소모품일때
        {
            for (int x = 0; x < itemEffects.Length; x++) //배열 탐색 , 아이템의 효과만큼
            {
                if (itemEffects[x].itemName == _item.itemName) //
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                
                                break;
                            case SP:
                                break;
                            case BATTERY:
                                break;
                            case WILL:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위.");
                                break;
                        }
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는 ItemName 못 찾음");
        }
    }
}
