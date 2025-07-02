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

        // 전체 가중치 반환
        foreach (DropInfo dropInfo in dropTable)
        {
            totalWeight += dropInfo.dropWeight;
        }

        // 확률 키 설정
        int r = Random.Range(0, totalWeight);

        // 위 키에 해당하는 아이템 반환
        foreach (DropInfo dropInfo in dropTable)
        {
            // 현재 키가, 해당 아이템의 확률(가중치) 보다 높으면
            key += dropInfo.dropWeight;
            if (r < key)
            {
                // 현재 키에 해당 드랍 정보 적용
                return dropInfo;
            }
        }

        Debug.LogError("드랍 오류 발생: 드랍테이블에 확률(가중치)를 입력하지 않았습니다.");
        return null;
    }
}

[System.Serializable]
public class DropInfo
{
    public Item dropItem;
    [Tooltip("확률(or 비율로 적어도 됨)")]
    public int dropCount;
    public int dropWeight;
}
