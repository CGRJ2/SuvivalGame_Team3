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

    /*public float GetHpMultiplier(MonsterSubType subType)
    {
        foreach (var entry in scalingList)
        {
            //Debug.Log($"[GetHpMultiplier] SO List - subType: {entry.subType}, hpMultiplier: {entry.hpMultiplier}");
        }
        var found = scalingList.Find(x => x.subType == subType);
        *//*if (found == null)
            //Debug.LogWarning($"[GetHpMultiplier] {subType}�� �ش��ϴ� ������ �����ϴ�. �⺻�� 1 ��ȯ");
        else*//*
            //Debug.Log($"[GetHpMultiplier] {subType} �� {found.hpMultiplier} ��ȯ");

        //Debug.Log($"[GetHpMultiplier] SO �ν��Ͻ�ID: {this.GetInstanceID()}, ���: {UnityEditor.AssetDatabase.GetAssetPath(this)}");
        foreach (var entry in scalingList)
        {
            //Debug.Log($"[GetHpMultiplier] SO List - subType: {entry.subType}, hpMultiplier: {entry.hpMultiplier}");
        }


        return found != null ? found.hpMultiplier : 1f;

    }*/
}
