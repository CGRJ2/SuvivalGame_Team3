using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "New DropTable")]
public class DropTable : ScriptableObject
{
    [field: SerializeField] public string DropTableName { get; private set; }

    public List<DropInfo> dropTable = new List<DropInfo>();

    protected void OnEnable()
    {
        DropTableName = this.name;
    }

    public DropInfo GetDropItemInfo()
    {
        int totalWeight = 0;
        int key = 0;

        // ��ü ����ġ ��ȯ
        foreach (DropInfo dropInfo in dropTable)
        {
            totalWeight += dropInfo.dropWeight;
        }

        // Ȯ�� Ű ����
        int r = Random.Range(0, totalWeight);

        // �� Ű�� �ش��ϴ� ������ ��ȯ
        foreach (DropInfo dropInfo in dropTable)
        {
            // ���� Ű��, �ش� �������� Ȯ��(����ġ) ���� ������
            key += dropInfo.dropWeight;
            if (r < key)
            {
                // ���� Ű�� �ش� ��� ���� ����
                return dropInfo;
            }
        }

        Debug.LogError("��� ���� �߻�: ������̺� Ȯ��(����ġ)�� �Է����� �ʾҽ��ϴ�.");
        return null;
    }
}

[System.Serializable]
public class DropInfo
{
    public Item dropItem;
    [Tooltip("Ȯ��(or ������ ��� ��)")]
    public int dropCount;
    public int dropWeight;
}
