using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JWH_DataManager : Singleton<JWH_DataManager>
{
    [Header("���� ����")]
    [SerializeField] public int maxSlotCount = 5;

    private PlayerManager pm;

    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }

    private string PathFor(int slot) =>
         Path.Combine(Application.persistentDataPath, $"save_{slot}.json");//���ϰ��
    private bool SlotExists(int slot) => File.Exists(PathFor(slot));//üũ
    private bool Valid(int slot) => slot >= 0 && slot < maxSlotCount;//üũ

    public void SaveData(int slot)//���峻��
    {
        if (!Valid(slot)) return;
        var snapshot = new PlayerCopy();
        snapshot.Bring(pm.instancePlayer.Status);
        File.WriteAllText(PathFor(slot), JsonUtility.ToJson(snapshot, true));
    }

    public void LoadData(int slot)//�ҷ����³���
    {
        if (!Valid(slot) || !SlotExists(slot)) return;
        var json = File.ReadAllText(PathFor(slot));
        var snapshot = JsonUtility.FromJson<PlayerCopy>(json);
        snapshot.Give(pm.instancePlayer.Status);
    }

    public void DeleteSlot(int slot)//���������
    {
        if (SlotExists(slot)) File.Delete(PathFor(slot));
    }

    public List<int> GetSavedSlots()//���ϸ��
    {
        var result = new List<int>();
        for (int i = 0; i < maxSlotCount; i++)
            if (SlotExists(i)) result.Add(i);
        return result;
    }
}
