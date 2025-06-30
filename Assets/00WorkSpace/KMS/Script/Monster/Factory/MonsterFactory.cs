using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    public static MonsterFactory Instance { get; private set; }

    [Header("Monster Factory Resources")]
    [SerializeField] private List<MonsterTypeSOEntry> typeStatList; // 인스펙터에서 할당

    [System.Serializable]
    public class MonsterTypeSOEntry //몬스터 타입 정보 SO
    {
        public MonsterType type;
        public MonsterTypeStatData statSO;
    }

    [System.Serializable]
    public class StageSOEntry //스테이지 정보 SO
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

        // 타입 SO 딕셔너리
        typeStatDict = new Dictionary<MonsterType, MonsterTypeStatData>();
        foreach (var entry in typeStatList)
            typeStatDict[entry.type] = entry.statSO;

        // 스테이지 SO 딕셔너리
        stageSODict = new Dictionary<string, StageMonsterScalingData>();
        foreach (var entry in stageSOList)
            stageSODict[entry.stageName] = entry.stageSO;
    }

    public BaseMonster SpawnMonster(BaseMonsterData data, Vector3 position, string stageName)
    {
        Debug.Log($"[Test] SpawnMonster에 넘길 stageName: {stageName}");
        if (data == null || data.monsterPrefab == null)
        {
            Debug.LogError("[MonsterFactory] 잘못된 몬스터 데이터 또는 프리팹 누락");
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
                Debug.LogWarning($"[MonsterFactory] {type} 타입 StatSO가 없습니다. 기본값 사용");
                typeStat = ScriptableObject.CreateInstance<MonsterTypeStatData>();
                typeStat.hpMultiplier = 1f;
                typeStat.attackPowerMultiplier = 1f;
                typeStat.moveSpeedMultiplier = 1f;
                typeStat.knockbackDistanceMultiplier = 1f;
            }
            else
            {
                Debug.Log($"[MonsterFactory] {stageName}에 대응하는 StageSO 사용");
            }

            Debug.Log($"[SpawnMonster:Debug] 전달받은 stageName: {stageName}");
            Debug.Log($"[MonsterFactory] 현재 등록된 StageSO 딕셔너리 키 목록: {string.Join(", ", stageSODict.Keys)}");

            StageMonsterScalingData stageStat = null;
            if (string.IsNullOrEmpty(stageName) || stageSODict.TryGetValue(stageName, out stageStat) == false)
            {
                Debug.LogWarning($"[MonsterFactory] {stageName}에 해당하는 StageSO가 없습니다. (딕셔너리 키 목록: {string.Join(", ", stageSODict.Keys)}) 기본값 사용");
                stageStat = ScriptableObject.CreateInstance<StageMonsterScalingData>();
            }


            monster.SetData(data, typeStat, stageStat);
            return monster;
        }

        Debug.LogError($"[MonsterFactory] BaseMonster 컴포넌트가 프리팹에 없습니다: {data.monsterPrefab.name}");
        return null;
    }
}
