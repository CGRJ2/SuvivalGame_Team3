using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeID;         //������ID
    public Item[] requiredItems;    //�ʿ��Ѿ�����
    public int[] requiredCounts;    //�ʿ�����ۼ���
    public Item outputItem;         //���������
    public int outputCount = 1;     //��������ۼ���
}
