using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //����Ÿ�
    private bool pickupActivated = false; //���� ������ �� true
    private RaycastHit hitInfo; //�浹ü ���� ����

    //������ ���̾ �����ϵ��� ���̾��ũ ����
    [SerializeField]
    private LayerMask LayerMask;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;


    private void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemInstance>().item.itemName + " ȹ���߽��ϴ�");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemInstance>().item);

                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, LayerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else 
        {
            InfoDisappear();
        }

    }

    private void ItemInfoAppear()
    { 
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemInstance>().item.itemName + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
