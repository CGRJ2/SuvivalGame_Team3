using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JWHDataTest : Singleton<JWHDataTest>
{
    private PlayerManager pm;

    [Header("저장 가능한 최대 슬롯 수")]
    public int maxSlotCount = 5;

    [Header("플레이어 복사 데이터")]
    public PlayerCopy playercopy = new PlayerCopy();

    private string GetSavePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"save_{slotIndex}.json");
    }

    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }

    public void SaveData(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        var status = PlayerManager.Instance.instancePlayer.Status;
        playercopy.Bring(status);

        string json = JsonUtility.ToJson(playercopy, true);
        File.WriteAllText(GetSavePath(slotIndex), json);
        Debug.Log($"[저장 완료] 슬롯 {slotIndex} 저장");
    }

    public void LoadData(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        string path = GetSavePath(slotIndex);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[불러오기 실패] 슬롯 {slotIndex}");
            return;
        }

        string json = File.ReadAllText(path);
        playercopy = JsonUtility.FromJson<PlayerCopy>(json);

        var status = PlayerManager.Instance.instancePlayer.Status;
        playercopy.Give(status);
        Debug.Log($"[불러오기 완료] 슬롯 {slotIndex} ");
    }

    public void DeleteSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        string path = GetSavePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[삭제 완료] 슬롯 {slotIndex} 삭제");
        }
        else
        {
            Debug.LogWarning($"[삭제 실패] 슬롯 {slotIndex} 없음");
        }
    }

    public List<int> GetSavedSlots()
    {
        List<int> saved = new List<int>();

        for (int i = 0; i < maxSlotCount; i++)
        {
            if (File.Exists(GetSavePath(i)))
                saved.Add(i);
        }

        return saved;
    }

    private bool IsValidSlot(int index)
    {
        if (index < 0 || index >= maxSlotCount)
        {
            Debug.LogWarning($"[슬롯오류] 0 이상 {maxSlotCount - 1} ");
            return false;
        }
        return true;
    }
}

