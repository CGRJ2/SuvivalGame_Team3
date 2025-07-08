using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/소비 아이템/투척 아이템")]

public class Item_Throwing : Item_Consumable, IEquipable
{

    
    

    // 던지기 실행
    public void ThrowingPrefab()
    {
        /*Transform playerTransform = PlayerManager.Instance.instancePlayer.transform;
        Instantiate(installedPrefab, playerTransform.position, playerTransform.rotation);*/
    }

    public virtual void OnAttackEffect()
    {
        /*InstallPrefab();
        Destroy(PlayerManager.Instance.instancePlayer.onHandInstance);*/
    }

}
