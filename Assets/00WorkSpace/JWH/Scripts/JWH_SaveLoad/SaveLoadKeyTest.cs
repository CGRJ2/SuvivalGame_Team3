using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadKeyTest : MonoBehaviour
{

    [Header("테스트용 슬롯 번호 (0 ~ maxSlotCount-1)")]
    public int testSlot = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            JWH_DataManager.Instance.SaveData(testSlot);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            JWH_DataManager.Instance.LoadData(testSlot);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            JWH_DataManager.Instance.DeleteSlot(testSlot);
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            List<int> slots = JWH_DataManager.Instance.GetSavedSlots();
            Debug.Log($"[저장된 슬롯 목록] {string.Join(", ", slots)}");
        }
    }
}

