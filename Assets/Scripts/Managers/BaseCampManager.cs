using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    // 참조한 다른 매니저들보다 후 순위에서 초기화 해야 함
    PlayerManager pm;
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }
}
