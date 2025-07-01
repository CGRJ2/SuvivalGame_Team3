using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New StageData", menuName = "New StageData")]
public class StageData : ScriptableObject
{
    public Sprite stageImage;
    public string stageName;
    public bool isUnlocked;
    public StageUnlockCondition unlockCondition;


    protected void OnEnable()
    {
        stageName = this.name;
        unlockCondition.needTimeState = new List<TimeZoneState>() { TimeZoneState.All };
    }
}

[System.Serializable]
public class StageUnlockCondition
{
    public List<ItemRequirement> needItemList;
    public List<TimeZoneState> needTimeState;

}
