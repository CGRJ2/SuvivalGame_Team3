using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_BaseCampData
{
    public ObservableProperty<int> CurrentCampLevel { get; private set; }


    public void LevelUp()
    {
        if (CurrentCampLevel.Value < BaseCampManager.Instance.MaxLevel)
            CurrentCampLevel.Value += 1;
        else Debug.Log("������ �Ұ� [���� : �̹� �ִ� �����Դϴ�.]");
    }
    
}
