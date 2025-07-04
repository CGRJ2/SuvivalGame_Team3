using System;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    [field: SerializeField] public int MaxLevel { get; private set; }

    public BaseCampInstance baseCampInstance;

    public TemporaryCampInstance currentTempCampInstance;

    public GameObject TemporaryCampInstance_Prefab;

    // [���̺� & �ε� ������]
    public BaseCampData baseCampData;

    // [���̺� & �ε� ������]
    public TempCampData tempCampData;

    [HideInInspector] public Item_Recipe[] allRecipeList;

    [HideInInspector] public BaseCampUpgradeCondition[] UpgradeConditions;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerManager.Instance.PlayerFaint(-99);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerManager.Instance.PlayerDead();
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            UseTempCampItem();
        }
        
    }


    public void Init()
    {
        base.SingletonInit();
        baseCampData = new BaseCampData();
        tempCampData = new TempCampData();
        InitUpgradeConditions();
        InitAllRecipes();

        TemporaryCampInstance_Prefab = Resources.Load<GameObject>("Prefabs/BaseCamp/TempBaseCampInstance");

        DataManager.Instance.loadedDataGroup.Subscribe(LoadCampData);
    }

    // ���̺� �� �ӽ���Ʈ ������
    public TempCampData GetTempCampData()
    {
        if (currentTempCampInstance == null) return new TempCampData();
        else return new TempCampData(currentTempCampInstance.transform);
    }

    public void LoadCampData(SaveDataGroup loadedData)
    {
        baseCampData = loadedData.baseCampData;
        tempCampData = loadedData.tempCampData;

        // ���� �ӽ���Ʈ �ν��Ͻ� ����
        if (currentTempCampInstance != null)
        {
            currentTempCampInstance.DestroyOnlyInstacne();
        }

        // ����� �ӽ�ķ�� �����Ͱ� ������ �ӽ���Ʈ �ν��Ͻ� ����
        if (tempCampData.isActive)
        {
            // ���� ������ ����Ʈ(������ ����� ���� ķ�� ��ġ)�� ���� ķ�� ������ ��ȯ
            SpawnTempBaseCampInstance(tempCampData.tempCampPosition, tempCampData.tempCampRotation);
        }

        // ������ ���� ķ���� ��ġ �̵�
        MoveToLastCamp(true);

        Debug.Log("���̽�ķ�� �Ŵ��� ������ ������ �Լ� �Ϸ�");
    }


    private void InitUpgradeConditions()
    {
        UpgradeConditions = Resources.LoadAll<BaseCampUpgradeCondition>("BaseCampUpgradeConditions");
        Array.Sort(UpgradeConditions, (a, b) => a.currentLevel.CompareTo(b.currentLevel));
    }

    private void InitAllRecipes()
    {
        allRecipeList = Resources.LoadAll<Item_Recipe>("ItemDatabase/99 Recipes");
        Array.Sort(allRecipeList, (a, b) => a.RecipeData.orderIndex.CompareTo(b.RecipeData.orderIndex));
    }

    public void LevelUp()
    {
        if (baseCampData.CurrentCampLevel.Value < MaxLevel)
            baseCampData.CurrentCampLevel.Value += 1;
        else Debug.Log("������ �Ұ� [���� : �̹� �ִ� �����Դϴ�.]");
    }

    public GameObject SpawnTempBaseCampInstance(Vector3 spawningPos, Quaternion spawningQuat)
    {
        GameObject tempCampInstance = Instantiate(TemporaryCampInstance_Prefab, spawningPos, spawningQuat);

        // ���� �ٸ� �ӽ�ķ���� �ִٸ� ����
        if (currentTempCampInstance != null)
            currentTempCampInstance.DestroyOnlyInstacne();

        // ���� ��ȯ�� �ӽ�ķ�� ����
        if (tempCampInstance.GetComponent<TemporaryCampInstance>() == null) { Debug.LogError("TemporaryCampInstance�� ���� �ָ� ���������� ������"); }
        currentTempCampInstance = tempCampInstance.GetComponent<TemporaryCampInstance>();

        // ��� ����
        Debug.Log("���� ķ�� ���� => ������ ��� ����");
        DataManager.Instance.SaveData(0);

        return tempCampInstance;
    }


    // ���������� ����� ķ���� �̵�
    public void MoveToLastCamp(bool isLoad)
    {
        Debug.Log("�÷��̾� ���� �̵� ����");

        // ���� ķ�� �����Ͱ� �ִٸ� 
        if (currentTempCampInstance != null)
        {
            Debug.Log($"����ķ���� �̵� => ��ġ : {currentTempCampInstance.respawnPoint}");
            PlayerManager.Instance.instancePlayer.Respawn(currentTempCampInstance.respawnPoint);

            // ������ �ε�� ����� �̵��� �ƴ϶�� -> ����ķ�� �Ҹ�(�ı� & ������ ����)
            if (!isLoad)
                currentTempCampInstance.DestroyWithData();
        }
        // ������ ���̽�ķ���� �̵�
        else
        {
            Debug.Log($"���̽� ķ���� �̵� => ��ġ : {baseCampInstance.respawnPoint}");
            PlayerManager.Instance.instancePlayer.Respawn(baseCampInstance.respawnPoint);
        }
    }

    public void UseTempCampItem()
    {
        // ����
        Transform playerTransform = PlayerManager.Instance.instancePlayer.transform;
        GameObject gameObject = SpawnTempBaseCampInstance(playerTransform.position, playerTransform.rotation);
        gameObject.transform.SetParent(null);

        
    }



}

