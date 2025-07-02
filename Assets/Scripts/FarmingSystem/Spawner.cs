using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ���� ����
    public SpawnerType spawnerType;

    // �ش� �����ʰ� ������ ���
    public StageData stageOrigin;

    // ���� ��ȯ�� �Ĺֿ�����Ʈ.. ��ȯ �ȵǾ� �ִ� ���¸� null��
    public GameObject currentSpawned_Instance;

    private void Awake() => Init();

    private void Init()
    {
        StageManager sm = StageManager.Instance;
        sm.GetSpawnerListGroup(stageOrigin.stageKey).AddToSpawnerList(this);
    }

    private SpawnTable GetSpawnTable()
    {
        // 1. �������� �������� ��� ���� �Ǵ�
        if (!stageOrigin.IsUnlocked) return null;

        // 2. ���� ���������� ������ ���� ���̺� Weight ���� Ȯ��
        if (spawnerType == SpawnerType.FO)
        {
            if (stageOrigin.SpawnTableGroup_FO.Count < 0) return null;
        }
        else if (spawnerType == SpawnerType.Monster)
        {
            if (stageOrigin.SpawnTableGroup_Monster.Count < 0) return null;
        }


        // 3. Origin�� �Ž�����, �ƴ��� Ȯ��
        List<TableInfoByWeight> tableInfos = null;
        // �Ž��̶�� �������� ������ �´� ���̺� ����ġ ����
        if (stageOrigin.stageKey == StageKey.�Ž�)
        {
            int currentStageLevel = StageManager.Instance.CurrentStageLevel.Value;

            if (spawnerType == SpawnerType.FO)
            {
                tableInfos = stageOrigin.SpawnTableGroup_FO[currentStageLevel].tableWeightInfos;
            }
            else if (spawnerType == SpawnerType.Monster)
            {
                tableInfos = stageOrigin.SpawnTableGroup_Monster[currentStageLevel].tableWeightInfos;
            }
        }
        // �Ž��� �ƴ϶�� ù ��° ���̺� ����ġ ����
        else
        {
            if (spawnerType == SpawnerType.FO)
            {
                tableInfos = stageOrigin.SpawnTableGroup_FO[0].tableWeightInfos;
            }
            else if (spawnerType == SpawnerType.Monster)
            {
                tableInfos = stageOrigin.SpawnTableGroup_Monster[0].tableWeightInfos;
            }
        }

        // ��ü ����ġ ����
        int totalWeight = 0;
        int key = 0;
        foreach (TableInfoByWeight twi in tableInfos)
        {
            totalWeight += twi.weight;
        }

        // Ȯ�� Ű ����
        int r = Random.Range(0, totalWeight);



        // Ȯ���� ���� ���̺� ����
        foreach (TableInfoByWeight twi in tableInfos)
        {
            key += twi.weight;
            if (r < key)
            {
                return twi.spawnTable;
            }
        }

        return null;
    }

    public GameObject GetSpawnStandByObject()
    {
        SpawnTable instanceSpawnTable = GetSpawnTable();
        if (instanceSpawnTable == null)
        {
            return null;
        }

        // ��ü ����ġ ����
        int totalWeight = 0;
        int key = 0;
        foreach (SpawnObjectInfo soi in instanceSpawnTable.objectList)
        {
            totalWeight += soi.objectWeight;
        }

        // Ȯ�� Ű ����
        int r = Random.Range(0, totalWeight);

        // Ȯ���� ���� ���̺� ����
        foreach (SpawnObjectInfo soi in instanceSpawnTable.objectList)
        {
            key += soi.objectWeight;
            if (r < key)
            {
                return soi.spawnObject;
            }
        }

        return null;
    }

    public void Spawn()
    {
        GameObject selectedPrefabs = GetSpawnStandByObject();
        if (selectedPrefabs != null)
        {
            // �ν��Ͻ� ��ȯ
            GameObject instance = Instantiate(selectedPrefabs, transform);

            // �ش� ����Ʈ �׷쿡 Ȱ���� ������Ʈ ����Ʈ ã��
            SpawnerListGroup tartgetSpawnerListGroup = StageManager.Instance.GetSpawnerListGroup(stageOrigin.stageKey);
            List<GameObject> targetActiveList = null;
            if (spawnerType == SpawnerType.FO)
                targetActiveList = tartgetSpawnerListGroup.activedFOInstances;
            else if (spawnerType == SpawnerType.Monster)
                targetActiveList =  tartgetSpawnerListGroup.activedMonsterInstances;

            // Ȱ���� ������Ʈ ����Ʈ�� �ν��Ͻ� �߰�
            if (targetActiveList != null)
                targetActiveList.Add(instance);

            // �ش� ������Ʈ �ı�(or ��Ȱ��ȭ) �� ����� Action�� (Ȱ���� ������Ʈ ����Ʈ�� �ν��Ͻ� ����) �Լ� ����
            instance.GetComponent<ISpawnable>().DeactiveAction = () =>
            {
                targetActiveList.Remove(instance);
            };

            currentSpawned_Instance = instance;
        }
        else
        {
            Debug.LogError("���õ� ������Ʈ �������� ����");
        }
    }
}
public enum SpawnerType { FO, Monster }
