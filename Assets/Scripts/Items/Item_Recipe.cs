using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/��� ������/������")]
public class Item_Recipe : Item_Ingredient
{
    protected override void OnEnable()
    {
        base.OnEnable();
        maxCount = 1;
    }

    // �ｺ�� ���ٿͼ� ������ ������ �����аŶ� �����ؼ� �����丵 �غ���
}
