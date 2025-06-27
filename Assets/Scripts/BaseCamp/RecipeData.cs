using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "New Recipe/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("�ݵ��! �ش� �����ǿ� �ش��ϴ� [������ ������]�� �̸��� �����ϰ� �ۼ����ּ���.")]
    public string recipeName;
    public Sprite icon;

    [Header("����� ����")]
    public Item resultItem;
    public int resultItemCount = 1;

    [Header("��� ����")]
    public List<ItemRequirement> requiredItems;


    [Header("��� ����")]
    public bool isUnlocked;

}
