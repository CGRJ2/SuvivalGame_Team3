using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    public static MonsterFactory Instance { get; private set; }

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
            Debug.LogError("[MonsterFactory] 잘못된 몬스터 데이터 또는 프리팹 누락");
            return null;
        }

        GameObject obj = Instantiate(data.monsterPrefab, position, Quaternion.identity);
        BaseMonster monster = obj.GetComponent<BaseMonster>();

        if (monster != null)
        {
            monster.SetData(data);
            return monster;
        }

        Debug.LogError($"[MonsterFactory] BaseMonster 컴포넌트가 프리팹에 없습니다: {data.monsterPrefab.name}");
        return null;
    }
}
