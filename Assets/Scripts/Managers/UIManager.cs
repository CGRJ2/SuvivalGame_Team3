using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject InventoryPanel;

    public void Init()
    {
        base.SingletonInit();
    }
}
