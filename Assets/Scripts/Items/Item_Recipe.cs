using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/재료 아이템/레시피")]
public class Item_Recipe : Item_Ingredient
{
    protected override void OnEnable()
    {
        base.OnEnable();
        maxCount = 1;
    }

    // 헬스장 갔다와서 레시피 데이터 만들어둔거랑 연결해서 리팩토링 해보자
}
