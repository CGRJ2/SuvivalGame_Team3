using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageUnlockCondition", menuName = "New StageUnlockCondition/StageUnlockCondition")]

public class StageUnlockCondition : ScriptableObject
{
    public ObservableProperty<bool> IsCleared { get; private set; }

    public List<ItemRequirement> needItemCheckList;

    public List<TimeZoneState> needTimeState;

}
