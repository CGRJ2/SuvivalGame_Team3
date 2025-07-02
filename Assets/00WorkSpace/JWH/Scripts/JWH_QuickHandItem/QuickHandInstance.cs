using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickHandInstance : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Transform handTransform;

    private Item lastItem;
    private GameObject currentHandObject;

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
        //if (item != null && item.itemName == "Test_Knife")//������ Ž��-���߿� �����ۿ��� ���� �ʵ带 �߰��ϴ°� ���� �������
        //{
        //    item.instancePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>
        //        ("Assets/00WorkSpace/JWH/Scripts/JWH_HandQuickView/Test_Knife.prefab");
        //}

        // ���� �� ������ ����
        if (currentHandObject != null)
        {
            Destroy(currentHandObject);
            currentHandObject = null;
        }

        if (item != null && item.instancePrefab != null)
        {
            currentHandObject = Instantiate(item.instancePrefab);
            currentHandObject.transform.SetParent(handTransform);
            currentHandObject.transform.localPosition = Vector3.zero;
            currentHandObject.transform.localRotation = Quaternion.identity;

            
            var rb = currentHandObject.GetComponent<Rigidbody>();// �ݶ��̴�/���� ����
            if (rb != null) rb.isKinematic = true;

            var col = currentHandObject.GetComponent<Collider>();
            if (col != null) col.enabled = false;

           
            var instance = currentHandObject.GetComponent<ItemInstance>();
            instance?.InitInstance(item, 1);
        }
    }
}