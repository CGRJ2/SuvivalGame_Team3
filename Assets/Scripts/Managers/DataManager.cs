using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    PlayerManager pm;

    [Header("���� ������ �ִ� ���� ��")]
    public int maxSlotCount = 5;

    public ObservableProperty<SaveDataGroup> loadedDataGroup = new ObservableProperty<SaveDataGroup>();

    /// <summary>
    /// �׽�Ʈ
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveData(0);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData(0);
        }
    }*/
    /// </summary>


    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }

    private string GetSavePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"save_{slotIndex}.json");
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

    public void SaveData(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        // ������ �����͵�
        SaveDataGroup instanceSaveDataGroup = new SaveDataGroup()
        {
            currentPosition = PlayerManager.Instance.instancePlayer.transform.position,
            playerStatusData = PlayerManager.Instance.instancePlayer.Status,
            inventoryModel = PlayerManager.Instance.instancePlayer.Status.inventory.model,
            slotDataListsData = PlayerManager.Instance.instancePlayer.Status.inventory.model.SaveSlotItemData(),
            currentTimeData = DailyManager.Instance.currentTimeData,
            stageUnlockData = StageManager.Instance.GetStageUnlockSaveData(),
            baseCampData = BaseCampManager.Instance.baseCampData,
            tempCampData = BaseCampManager.Instance.GetTempCampData(),
            // �̰� �� �ν��Ͻ��� ����� ����??? ����� Ŭ������ �ҷ��� ��, null���� �⺻������ ��ü�ǳ�?
        };


        Debug.LogWarning(instanceSaveDataGroup.tempCampData);
        string json = JsonUtility.ToJson(instanceSaveDataGroup, true);
        File.WriteAllText(GetSavePath(slotIndex), json);

        UIManager.Instance.popUpUIGroup.PopMessage(UIManager.Instance.popUpUIGroup.message_Saved);
    }
    

    public SaveDataGroup GetLoadData(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return null;

        string path = GetSavePath(slotIndex);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[�ҷ����� ����] ���� {slotIndex}");
            return null;
        }

        string json = File.ReadAllText(path);
        Debug.Log($"[�ҷ����� �Ϸ�] ���� {slotIndex} ");
        return JsonUtility.FromJson<SaveDataGroup>(json);
    }

    public void LoadData(int slotIndex)
    {
        loadedDataGroup.Value = GetLoadData(slotIndex);
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

}

[System.Serializable]
public class SaveDataGroup
{
    public Vector3 currentPosition;
    public PlayerStatus playerStatusData;
    public InventoryModel inventoryModel;
    public SlotDataListsData slotDataListsData;
    public CurrentTimeData currentTimeData;
    public List<bool> stageUnlockData;
    public BaseCampData baseCampData;
    public TempCampData tempCampData;
}

