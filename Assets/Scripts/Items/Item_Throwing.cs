using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/�Һ� ������/��ô ������")]

public class Item_Throwing : Item_Consumable, IEquipable
{

    
    

    // ������ ����
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
