using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    PlayerManager pm;
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }


    public void SaveData()
    {
        PlayerStatus status = pm.instancePlayer.Status;
    }

    public void LoadData()
    {
        PlayerStatus status = pm.instancePlayer.Status;
    }
}
