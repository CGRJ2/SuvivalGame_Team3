using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/StageScalingData")]
public class StageMonsterScalingData : ScriptableObject
{
    public int stageIndex;

    [System.Serializable]
    public class SubTypeScaling
    {
        public MonsterSubType subType;
        public float attackMultiplier = 1f;
        public float hpMultiplier = 1f;
    }

    [SerializeField]
    private List<SubTypeScaling> scalingList = new();

    public float GetAttackMultiplier(MonsterSubType subType)
    {
        var entry = scalingList.Find(x => x.subType == subType);
        return entry != null ? entry.attackMultiplier : 1f;
    }

    public float GetHpMultiplier(MonsterSubType subType)
    {
        var entry = scalingList.Find(x => x.subType == subType);
        return entry != null ? entry.hpMultiplier : 1f;
    }
}
