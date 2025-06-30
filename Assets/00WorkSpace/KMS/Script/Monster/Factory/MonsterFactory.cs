using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    public static MonsterFactory Instance { get; private set; }

    [Header("Monster Factory Resources")]
    [SerializeField] private List<MonsterTypeSOEntry> typeStatList; // �ν����Ϳ��� �Ҵ�

    [System.Serializable]
    public class MonsterTypeSOEntry //���� Ÿ�� ���� SO
    {
        public MonsterType type;
        public MonsterTypeStatData statSO;
    }

    [System.Serializable]
    public class StageSOEntry //�������� ���� SO
    {
        public string stageName;
        public StageMonsterScalingData stageSO;
    }

    [SerializeField] private List<StageSOEntry> stageSOList;
    private Dictionary<string, StageMonsterScalingData> stageSODict;
    private Dictionary<MonsterType, MonsterTypeStatData> typeStatDict;
    [SerializeField] private StageMonsterScalingData currentStageScaling;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Ÿ�� SO ��ųʸ�
        typeStatDict = new Dictionary<MonsterType, MonsterTypeStatData>();
        foreach (var entry in typeStatList)
            typeStatDict[entry.type] = entry.statSO;

        // �������� SO ��ųʸ�
        stageSODict = new Dictionary<string, StageMonsterScalingData>();
        foreach (var entry in stageSOList)
            stageSODict[entry.stageName] = entry.stageSO;
    }

    public BaseMonster SpawnMonster(BaseMonsterData data, Vector3 position, string stageName)
    {
        Debug.Log($"[Test] SpawnMonster�� �ѱ� stageName: {stageName}");
        if (data == null || data.monsterPrefab == null)
        {
            Debug.LogError("[MonsterFactory] �߸��� ���� ������ �Ǵ� ������ ����");
            return null;
        }

        GameObject obj = Instantiate(data.monsterPrefab, position, Quaternion.identity);
        BaseMonster monster = obj.GetComponent<BaseMonster>();

        if (monster != null)
        {
            MonsterType type = data.monsterType;
            MonsterTypeStatData typeStat = null;

            if (typeStatDict.TryGetValue(type, out typeStat) == false)
            {
                Debug.LogWarning($"[MonsterFactory] {type} Ÿ�� StatSO�� �����ϴ�. �⺻�� ���");
                typeStat = ScriptableObject.CreateInstance<MonsterTypeStatData>();
                typeStat.hpMultiplier = 1f;
                typeStat.attackPowerMultiplier = 1f;
                typeStat.moveSpeedMultiplier = 1f;
                typeStat.knockbackDistanceMultiplier = 1f;
            }
            else
            {
                Debug.Log($"[MonsterFactory] {stageName}�� �����ϴ� StageSO ���");
            }

            Debug.Log($"[SpawnMonster:Debug] ���޹��� stageName: {stageName}");
            Debug.Log($"[MonsterFactory] ���� ��ϵ� StageSO ��ųʸ� Ű ���: {string.Join(", ", stageSODict.Keys)}");

            StageMonsterScalingData stageStat = null;
            if (string.IsNullOrEmpty(stageName) || stageSODict.TryGetValue(stageName, out stageStat) == false)
            {
                Debug.LogWarning($"[MonsterFactory] {stageName}�� �ش��ϴ� StageSO�� �����ϴ�. (��ųʸ� Ű ���: {string.Join(", ", stageSODict.Keys)}) �⺻�� ���");
                stageStat = ScriptableObject.CreateInstance<StageMonsterScalingData>();
            }


            monster.SetData(data, typeStat, stageStat);
            return monster;
        }

        Debug.LogError($"[MonsterFactory] BaseMonster ������Ʈ�� �����տ� �����ϴ�: {data.monsterPrefab.name}");
        return null;
    }
}
