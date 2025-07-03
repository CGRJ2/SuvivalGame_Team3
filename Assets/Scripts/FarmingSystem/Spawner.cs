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


    // ������ ��ƾ ����
    public Coroutine spawnRoutineInProgress;

    private void Awake() => InitToList();

    private void InitToList()
    {
        StageManager sm = StageManager.Instance;
        sm.GetSpawnerListGroup(stageOrigin.stageKey).AddToSpawnerList(this);
    }

    public void Init()
    {
        DestroySpawnedObject();
        CancelSpawning();
    }

    private void OnDisable()
    {
        CancelSpawning();
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
            int currentStageLevel = StageManager.Instance.CurrentStageLevel;

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

    private GameObject GetSpawnStandByObject()
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

    private void Spawn()
    {
        GameObject selectedPrefabs = GetSpawnStandByObject();

        if (selectedPrefabs != null)
        {
            // �ν��Ͻ� ��ȯ
            GameObject instance = Instantiate(selectedPrefabs, transform);
            ISpawnable spawnable = instance.GetComponent<ISpawnable>();

            // ����ó��
            if (spawnable == null) { Debug.LogError("ISpawnable �������̽��� ���� ������Ʈ�� ��ȯ�Ϸ���!"); return; }

            // �ش� ����Ʈ �׷쿡 Ȱ���� ������Ʈ ����Ʈ ã��
            SpawnerListGroup tartgetSpawnerListGroup = StageManager.Instance.GetSpawnerListGroup(stageOrigin.stageKey);
            List<GameObject> targetActiveList = null;
            if (spawnerType == SpawnerType.FO)
                targetActiveList = tartgetSpawnerListGroup.activateInstances_FO;
            else if (spawnerType == SpawnerType.Monster)
                targetActiveList =  tartgetSpawnerListGroup.activateInstances_Monster;

            // Ȱ���� ������Ʈ ����Ʈ�� �ν��Ͻ� �߰�
            if (targetActiveList != null)
                targetActiveList.Add(instance);
            
            currentSpawned_Instance = instance;

            // �ش� ������Ʈ �ı�(or ��Ȱ��ȭ) �� ����� Action�� (Ȱ���� ������Ʈ ����Ʈ�� �ν��Ͻ� ����) �Լ� ����
            spawnable.DeactiveAction = () =>
            {
                spawnable.OriginTransform = transform;
                targetActiveList.Remove(instance);
                currentSpawned_Instance = null;
            };
        }
        else
        {
            Debug.LogError($"���õ� ������Ʈ �������� ����! ���� �߻� ��ġ : {gameObject.name}({spawnerType}Ÿ�� ������)");
        }
    }

    // ������ ������ ������ ��Ŀ����
    private IEnumerator SpawnRoutine(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        Spawn();

        // ���� �Ϸ� �� �ڷ�ƾ ����
        spawnRoutineInProgress = null;
    }

    public void StartSpawning(float respawnTime)
    {
        if (spawnRoutineInProgress == null)
            spawnRoutineInProgress = StartCoroutine(SpawnRoutine(respawnTime));
    }

    public void CancelSpawning()
    {
        if (spawnRoutineInProgress != null)
        {
            StopCoroutine(spawnRoutineInProgress);
            spawnRoutineInProgress = null;
        }
    }

    public void DestroySpawnedObject()
    {
        if (currentSpawned_Instance != null) Destroy(currentSpawned_Instance);
    }
}
public enum SpawnerType { FO, Monster }
