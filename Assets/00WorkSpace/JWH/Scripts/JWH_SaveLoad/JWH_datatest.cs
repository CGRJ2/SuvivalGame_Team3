using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JWHDataTest : Singleton<JWHDataTest>
{
    private PlayerManager pm;

    [Header("���� ������ �ִ� ���� ��")]
    public int maxSlotCount = 5;

    [Header("�÷��̾� ���� ������")]
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
        Debug.Log($"[���� �Ϸ�] ���� {slotIndex} ����");
    }

    public void LoadData(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        string path = GetSavePath(slotIndex);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[�ҷ����� ����] ���� {slotIndex}");
            return;
        }

        string json = File.ReadAllText(path);
        playercopy = JsonUtility.FromJson<PlayerCopy>(json);

        var status = PlayerManager.Instance.instancePlayer.Status;
        playercopy.Give(status);
        Debug.Log($"[�ҷ����� �Ϸ�] ���� {slotIndex} ");
    }

    public void DeleteSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        string path = GetSavePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[���� �Ϸ�] ���� {slotIndex} ����");
        }
        else
        {
            Debug.LogWarning($"[���� ����] ���� {slotIndex} ����");
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
            Debug.LogWarning($"[���Կ���] 0 �̻� {maxSlotCount - 1} ");
            return false;
        }
        return true;
    }
}

