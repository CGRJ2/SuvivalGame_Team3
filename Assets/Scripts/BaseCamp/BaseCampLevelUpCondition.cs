using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BaseCampLevelUpCondition", menuName = "New BaseCampLevelUpCondition/BaseCampLevelUpCondition")]

public class BaseCampLevelUpCondition : ScriptableObject
{
    public int currentLevel;

    public List<ItemRequirement> needItemCheckList;
}
