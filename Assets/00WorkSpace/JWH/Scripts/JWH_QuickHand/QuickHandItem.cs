using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickHandItem : MonoBehaviour
{

    [SerializeField] private Transform handTransform;// 손위치 변경시 사용 추천위치:B-palm_01_R
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
            Destroy(curHandObject);// 기존 손 아이템 제거
            curHandObject = null;
        }

        if (item != null && item.instancePrefab != null)
        {
            curHandObject = Instantiate(item.instancePrefab);
            Transform grip = curHandObject.transform.Find("GripPoint");//프리팹마다 Grip라는 transform으로 잡는위치 할당
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
                Debug.LogWarning($"GripPoint가 없음 {item.itemName}");
                //curHandObject.transform.SetParent(handTransform);
                //curHandObject.transform.localPosition = Vector3.zero;
                //curHandObject.transform.localRotation = Quaternion.identity;
                curHandObject.transform.SetParent(handTransform, worldPositionStays: false);

                // 프리팹 손위치 기본값 z손에서다리, x손에서어깨, y손에서 아이템 잡는 위치
                curHandObject.transform.localPosition = new Vector3(-0.05f, -0.2f, 0.07f); // 손잡이 위치
                curHandObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // 회전
            }

            //var rb = curHandObject.GetComponent<Rigidbody>();//물리제거
            //if (rb != null) rb.isKinematic = true;
            //var col = curHandObject.GetComponent<Collider>();
            //if (col != null) col.enabled = false;

            var instance = curHandObject.GetComponent<ItemInstance>();
            instance?.InitInstance(item, 1);
        }
    }
}