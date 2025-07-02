using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spanwer : MonoBehaviour
{
    // ���� ��ȯ�� �Ĺֿ�����Ʈ.. ��ȯ �ȵǾ� �ִ� ���¸� null��
    public GameObject currentSpawned_Instance;

    // �ش� �����ʰ� ������ ���
    public StageData stageOrigin;

    ////////////////////////////
    ///���� �Ұ�
    ///�����ʵ� �ʱ�ȭ �� => �������� �Ŵ��� �ȿ� ���� �����ʵ� ����Ʈ�� �ְ�!
    ///�����ʵ��� ������ �Ĺֿ�����Ʈ�� �ڽ��� Origin������ �´� StageData�� farmingObjectList�� �޾� �� �ȿ��� �������� ������Ʈ �ϳ� ����
    ///������ ����Ʈ�� �߿��� �������� �����ϰ� �����


    //////////////////////�׽�Ʈ/////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn();
        }
    }
    //////////////////////�׽�Ʈ/////////////////////////////////


    private SpawnTable GetSpawnTable()
    {
        Debug.Log("0");

        // 1. �������� �������� ��� ���� �Ǵ�
        if (!stageOrigin.IsUnlocked) return null;
        Debug.Log("1");

        // 2. ���� ���������� ������ ���� ���̺� Weight ���� Ȯ��
        if (stageOrigin.SpawnTableGroup_FO.Count < 0) return null;
        Debug.Log("2");

        // 3. Origin�� �Ž�����, �ƴ��� Ȯ��
        List<TableInfoByWeight> tableInfos = null;
        // �Ž��̶�� �������� ������ �´� ���̺� ����ġ ����
        if (stageOrigin.stageKey == StageKey.�Ž�)
        {
            int currentStageLevel = StageManager.Instance.CurrentStageLevel.Value;
            tableInfos = stageOrigin.SpawnTableGroup_FO[currentStageLevel].tableWeightInfos;
        }
        // �Ž��� �ƴ϶�� ù ��° ���̺� ����ġ ����
        else
        {
            tableInfos = stageOrigin.SpawnTableGroup_FO[0].tableWeightInfos;
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

        Debug.Log("Ȯ�� ��� ������ ������");


        // Ȯ���� ���� ���̺� ����
        foreach (TableInfoByWeight twi in tableInfos)
        {
            key += twi.weight;
            if (r < key)
            {
                Debug.Log("���� ���̺� ��");
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
            Debug.LogError("�������̺��� �����ϴ�");
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
        GameObject instance = GetSpawnStandByObject();
        if (instance != null)
            currentSpawned_Instance = Instantiate(instance, transform.position, transform.rotation);
    }
}
