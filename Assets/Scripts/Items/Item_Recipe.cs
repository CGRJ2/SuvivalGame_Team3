using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/��� ������/������")]
public class Item_Recipe : Item_Ingredient
{
    public string recipeName;
    public Sprite icon;

    [field: Header("������ ������")]
    [field: SerializeField] public RecipeData RecipeData { get; private set; }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        maxCount = 1;
    }

    
    public void UnlockThisRecipe()
    {
        if (!RecipeData.isUnlocked)
            RecipeData.isUnlocked = true;
        else
        {
            Debug.Log("�̹� ���۴뿡 �߰��� �������̴�.");
        }
    }
}

[System.Serializable]
public class RecipeData
{
    [Header("��� ����")]
    public bool isUnlocked;

    [Header("������ â�� ��Ÿ�� ����")]
    public int orderIndex;

    [Header("���� �ð�")]
    public float craftDuration = 3f;

    [Header("����� ����")]
    public Item resultItem;
    public int resultItemCount = 1;

    [Header("��� ����")]
    public List<ItemRequirement> requiredItems;
}
