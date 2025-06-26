using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    public static MonsterFactory Instance { get; private set; }

    [Header("Monster Factory Resources")]
    [SerializeField] private MonsterTypeStatData statDatabase;
    [SerializeField] private StageMonsterScalingData currentStageScaling;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public BaseMonster SpawnMonster(BaseMonsterData data, Vector3 position)
    {
        if (data == null || data.monsterPrefab == null)
        {
            Debug.LogError("[MonsterFactory] �߸��� ���� ������ �Ǵ� ������ ����");
            return null;
        }

        GameObject obj = Instantiate(data.monsterPrefab, position, Quaternion.identity);
        BaseMonster monster = obj.GetComponent<BaseMonster>();

        if (monster != null)
        {
            var typeStat = data.typeStatData; 
            var stageStat = currentStageScaling;

            if (typeStat == null || stageStat == null)
            {
                Debug.LogWarning($"[MonsterFactory] ���� ������ �����ϴ�. �⺻���� ����մϴ�.");

                typeStat ??= ScriptableObject.CreateInstance<MonsterTypeStatData>();
                typeStat.hpMultiplier = 1f;
                typeStat.attackPowerMultiplier = 1f;
                typeStat.moveSpeedMultiplier = 1f;
                typeStat.knockbackDistanceMultiplier = 1f;

                stageStat ??= ScriptableObject.CreateInstance<StageMonsterScalingData>();
            }

            monster.SetData(data, typeStat, stageStat);
            return monster;
        }

        Debug.LogError($"[MonsterFactory] BaseMonster ������Ʈ�� �����տ� �����ϴ�: {data.monsterPrefab.name}");
        return null;
    }
}
