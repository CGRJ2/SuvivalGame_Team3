using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickHandItem : MonoBehaviour
{

    [SerializeField] private Transform handTransform;// ����ġ ����� ��� ��õ��ġ:B-palm_01_R
    private PlayerStatus playerStatus;
    private Item lastItem;
    private GameObject curHandObject;

    void Update()
    {
        if (playerStatus == null)
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.instancePlayer != null)
            {
                playerStatus = PlayerManager.Instance.instancePlayer.Status;
            }
        }

        Item curItem = playerStatus.onHandItem;
        if (curItem != lastItem)
        {
            UpdateHandItem(curItem);
            lastItem = curItem;
        }
    }

    private void UpdateHandItem(Item item)
    {
        if (curHandObject != null)
        {
            Destroy(curHandObject);// ���� �� ������ ����
            curHandObject = null;
        }

        if (item != null && item.instancePrefab != null)
        {
            curHandObject = Instantiate(item.instancePrefab);
            Transform grip = curHandObject.transform.Find("GripPoint");//�����ո��� Grip��� transform���� �����ġ �Ҵ�
            if (grip != null)
            {
                curHandObject.transform.SetParent(handTransform, worldPositionStays: false);
                Vector3 gripOffset = handTransform.position - grip.position;
                Quaternion gripRotOffset = handTransform.rotation * Quaternion.Inverse(grip.rotation);
                curHandObject.transform.position += gripOffset;
                curHandObject.transform.rotation = gripRotOffset * curHandObject.transform.rotation;
            }
            else
            {
                Debug.LogWarning($"GripPoint�� ���� {item.itemName}");
                //curHandObject.transform.SetParent(handTransform);
                //curHandObject.transform.localPosition = Vector3.zero;
                //curHandObject.transform.localRotation = Quaternion.identity;
                curHandObject.transform.SetParent(handTransform, worldPositionStays: false);

                // ������ ����ġ �⺻�� z�տ����ٸ�, x�տ������, y�տ��� ������ ��� ��ġ
                curHandObject.transform.localPosition = new Vector3(-0.05f, -0.2f, 0.07f); // ������ ��ġ
                curHandObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // ȸ��
            }

            //var rb = curHandObject.GetComponent<Rigidbody>();//��������
            //if (rb != null) rb.isKinematic = true;
            //var col = curHandObject.GetComponent<Collider>();
            //if (col != null) col.enabled = false;

            var instance = curHandObject.GetComponent<ItemInstance>();
            instance?.InitInstance(item, 1);
        }
    }
}