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
            Debug.LogError("[MonsterFactory] �߸��� ���� ������ �Ǵ� ������ ����");
            return null;
        }

        GameObject obj = Instantiate(data.monsterPrefab, position, Quaternion.identity);
        BaseMonster monster = obj.GetComponent<BaseMonster>();

        if (monster != null)
        {
            monster.SetData(data);
            return monster;
        }

        Debug.LogError($"[MonsterFactory] BaseMonster ������Ʈ�� �����տ� �����ϴ�: {data.monsterPrefab.name}");
        return null;
    }
}
