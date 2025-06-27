using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerStatus currentPlayerStatus;

    public void Init()
    {
        base.SingletonInit();
    }
}
