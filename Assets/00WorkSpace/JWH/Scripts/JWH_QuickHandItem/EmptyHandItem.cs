using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyHandItem : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    //[SerializeField] private Transform handTransform;
    [SerializeField] private GameObject EmptyHandPrefab;

    private GameObject EmptyHandInstance;
    private Item lastItem;

    void Start()
    {
        // 1È¸¸¸ ²®µ¥±â »ý¼º
        EmptyHandInstance = Instantiate(EmptyHandPrefab);
        EmptyHandInstance.transform.SetParent(transform, false);
        //EmptyHandInstance.transform.localPosition = Vector3.zero;
        //EmptyHandInstance.transform.localRotation = Quaternion.identity;

        UpdateEmptyHand(playerStatus.onHandItem);
    }

    void Update()
    {
        if (playerStatus.onHandItem != lastItem)
        {
            UpdateEmptyHand(playerStatus.onHandItem);
            lastItem = playerStatus.onHandItem;
        }
    }

    void UpdateEmptyHand(Item item)
    {
        var inst = EmptyHandInstance.GetComponent<ItemInstance>();
        if (inst != null)
        {
            inst.InitInstance(item, 1);
        }

        SwapVisual(item);
    }

    void SwapVisual(Item item)
    {
        Transform visualSlot = EmptyHandInstance.transform.Find("EmptyHand");
        if (visualSlot == null)
        {
            Debug.LogError("EmptyHand ½½·Ô ºö");
            return;
        }

        // ±âÁ¸ ¸ðµ¨ Á¦°Å
        foreach (Transform child in visualSlot)
            Destroy(child.gameObject);

        if (item != null && item.instancePrefab != null)
        {
            GameObject visual = Instantiate(item.instancePrefab, visualSlot);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;

            var col = visual.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            var rb = visual.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }
}