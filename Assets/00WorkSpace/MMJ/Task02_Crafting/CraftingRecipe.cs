using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;       //�������̸�
    public Sprite icon;             //������ �̹���


    public Item[] requiredItems;    //�ʿ��Ѿ�����
    public int[] requiredCounts;    //�ʿ�����ۼ���


    public Item resultItem;         //���������
    public int resultCount = 1;         //��������ۼ���

    public float craftDuration = 3f; // ���ۿ� �ɸ��� �ð� (�� ����)
}
