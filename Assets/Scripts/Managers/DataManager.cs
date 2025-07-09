using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    PlayerManager pm;

    [Header("저장 가능한 최대 슬롯 수")]
    public int maxSlotCount = 5;

    public ObservableProperty<SaveDataGroup> loadedDataGroup = new ObservableProperty<SaveDataGroup>();

    /// <summary>
    /// 테스트
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
            Debug.LogWarning($"[슬롯오류] 0 이상 {maxSlotCount - 1} ");
            return false;
        }
        return true;
    }

    public void SaveData(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return;

        // 저장할 데이터들
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
            // 이거 왜 인스턴스가 생기는 거죠??? 저장된 클래스를 불러올 때, null값은 기본값으로 대체되나?
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
            Debug.LogWarning($"[불러오기 실패] 슬롯 {slotIndex}");
            return null;
        }

        string json = File.ReadAllText(path);
        Debug.Log($"[불러오기 완료] 슬롯 {slotIndex} ");
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

