using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [SerializeField] private float throwForce = 10f; // ������ ��
    [SerializeField] private float destroyDelay = 3f; // �Ҹ� �ð�(��)
    [SerializeField] private Transform throwPosition; // ������ ��ġ (���� ī�޶� �Ǵ� �� ��ġ)
    [SerializeField] private QuickSlotParent quickSlotParent;
    

    private void Start()
    {
        //quickSlotParent = UIManager.Instance.inventoryGroup.quickSlotParent; // �̻��ϰ� �̰ɷ� �ϸ� �������� ���� �����ؼ� �������...

        if (quickSlotParent == null)
        {
            Debug.LogError("quickSlotParent is null!");
            return;
        }

        if (throwPosition == null)
        {
            throwPosition = Camera.main.transform;
            if (throwPosition == null)
            {
                Debug.LogError("Camera.main is null!");
            }
        }
    }

    // Update �޼��忡�� �Է� ó��
    private void Update()
    {

        // UI ��� ���� ���콺�� �ִ��� Ȯ���ϰ� �ִٸ� ������ ����
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // ���콺 ��Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            // item üũ
            if (quickSlotParent.NowSelectedSlot.slotData.item != null)
            {
                ThrowCurrentItem();
            }
        }
    }

    
    public void ThrowCurrentItem() // ���� ���õ� �������� ������ ������
    {
        // ���� ���õ� ���� Ȯ��
        QuickSlot currentSlot = quickSlotParent.NowSelectedSlot;

        // ���Կ� �������� �ְ� Item_Throwing Ÿ������ Ȯ��
        if (currentSlot != null && currentSlot.slotData.item != null && currentSlot.slotData.item is Item_Throwing throwingItem)
        {
            // ������ ������ ���� ��ġ
            Vector3 spawnPosition = throwPosition.position + throwPosition.forward * 0.5f;

            // �������� ������ �ν��Ͻ�ȭ
            GameObject thrownObject = Instantiate(throwingItem.instancePrefab, spawnPosition, throwPosition.rotation);

            // Rigidbody ������Ʈ �������� �Ǵ� �߰�
            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = thrownObject.AddComponent<Rigidbody>();
            }

            // �� �������� �� ���ϱ�
            rb.AddForce(throwPosition.forward * throwForce, ForceMode.Impulse);

            // ���� �ð� �� ������Ʈ �ı�
            Destroy(thrownObject, destroyDelay);

            // Item_Throwing�� OnAttackEffect �޼��� ȣ�� (�ʿ��)
            throwingItem.OnAttackEffect();

            // ������ �Һ� ó��
            throwingItem.Consume(currentSlot.slotData);

            // ���� UI ������Ʈ
            currentSlot.SlotViewUpdate();
        }
    }
}
