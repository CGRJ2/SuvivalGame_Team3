using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //�������� �̸�(Ű��)
    public string[] part; //����
    public int[] num; //��ġ

}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    //�ʿ��� ������Ʈ -> �÷��̾��� ü�°� ���ŷ� ��ġ, ���͸�� ���ؼ� ������ �޾ƿ;� ��
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
        if (_item.itemType == Item.ItemType.Used) //�Ҹ�ǰ�϶�
        {
            for (int x = 0; x < itemEffects.Length; x++) //�迭 Ž�� , �������� ȿ����ŭ
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
                                Debug.Log("�߸��� Status ����.");
                                break;
                        }
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� ItemName �� ã��");
        }
    }
}
