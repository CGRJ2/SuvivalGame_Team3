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


    public SpawnTable GetSpawnTable()
    {
        // 1. �������� �������� ��� ���� �Ǵ�
        if (!stageOrigin.IsUnlocked) return null;

        // 2. ���� ���������� ������ ���� ���̺� Weight ���� Ȯ��
        if (stageOrigin.SpawnTableGroup_FO.Count < 0) return null;

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
}
