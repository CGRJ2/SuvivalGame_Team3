using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickHandItem : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Transform handTransform;

    private Item lastItem;
    private GameObject curHandObject;

    void Update()
    {
        Item curItem = playerStatus.onHandItem;
        if (curItem != lastItem)
        {
            UpdateHandItem(curItem);
            lastItem = curItem;
        }
    }

    private void UpdateHandItem(Item item)
    {
        //if (item != null && item.itemName == "연습무기")//프리팹 탐색-나중에 아이템에서 전용 필드를 추가하는게 가장 쉬운듯함
        //{
        //    item.instancePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>
        //        ("Assets/00WorkSpace/JWH/Scripts/JWH_QuickHandItem/연습무기.prefab");
        //}

        // 기존 손 아이템 제거
        if (curHandObject != null)
        {
            Destroy(curHandObject);
            curHandObject = null;
        }

        if (item != null && item.instancePrefab != null)
        {
            curHandObject = Instantiate(item.instancePrefab);
            curHandObject.transform.SetParent(handTransform);
            curHandObject.transform.localPosition = Vector3.zero;
            curHandObject.transform.localRotation = Quaternion.identity;

            
            var rb = curHandObject.GetComponent<Rigidbody>();// 콜라이더/물리 꺼줌
            if (rb != null) rb.isKinematic = true;

            var col = curHandObject.GetComponent<Collider>();
            if (col != null) col.enabled = false;

           
            var instance = curHandObject.GetComponent<ItemInstance>();
            instance?.InitInstance(item, 1);
        }
    }
}