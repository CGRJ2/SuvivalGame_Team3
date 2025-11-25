using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManagerTest : Singleton<DataManagerTest>
{
    PlayerManager pm;
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }


    public void SaveData()
    {
        PlayerModel status = pm.instancePlayer.Status;
    }

    public void LoadData()
    {
        PlayerModel status = pm.instancePlayer.Status;
    }
}
