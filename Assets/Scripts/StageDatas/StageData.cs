using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New StageData", menuName = "New StageData")]
public class StageData : ScriptableObject
{
    [field: SerializeField] public bool IsUnlocked { get; private set; }
    [field: SerializeField] public int StageLevel { get; private set; }
    [field: SerializeField] public Sprite StageImage { get; private set; }
    [field: SerializeField] public string StageName { get; private set; }

    public StageUnlockCondition unlockCondition;


    protected void OnEnable()
    {
        StageName = this.name;
        unlockCondition.needTimeState = new List<TimeZoneState>() { TimeZoneState.All };
    }

    public void UlockStage()
    {
        IsUnlocked = true;
        StageManager.Instance.SetCurrentStageIndex(StageLevel);
    }
}

[System.Serializable]
public class StageUnlockCondition
{
    public List<ItemRequirement> needItemList;
    public List<TimeZoneState> needTimeState;

}
