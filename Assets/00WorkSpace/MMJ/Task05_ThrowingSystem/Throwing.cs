using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [SerializeField] private float throwForce = 10f; // ������ ��
    [SerializeField] private float destroyDelay = 3f; // �Ҹ� �ð�(��)
    [SerializeField] private Transform throwPosition; // ������ ��ġ (���� ī�޶� �Ǵ� �� ��ġ)
    [SerializeField] private QuickSlotParent quickSlotParent;

    // �� ���� ���� ������
    [Header("Throw Force Control")]
    [SerializeField] private float minThrowForce = 5f;  // �ּ� ������ ��
    [SerializeField] private float maxThrowForce = 30f; // �ִ� ������ ��
    [SerializeField] private float chargeRate = 20f;    // �ʴ� �� ������

    // ���� �̸����� ���� ������
    [Header("Trajectory Prediction")]
    [SerializeField] private LineRenderer trajectoryLine; // Line Renderer ������Ʈ
    [SerializeField] private int linePointCount = 50;     // ������ �׸� ���� ����
    [SerializeField] private float lineTimeStep = 0.1f;   // �� �� ������ �ð� ����

    private bool isCharging = false;
    private float currentChargeTime = 0f;

    private void Start()
    {
        throwPosition = transform;
        quickSlotParent = UIManager.Instance.inventoryGroup.quickSlotParent;
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

        // ���� ���� �ʱ�ȭ (���� �� ����)
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }
    }

    // Update �޼��忡�� �Է� ó��
    private void Update()
    {
        // UI ��� ���� ���콺�� �ִ��� Ȯ���ϰ� �ִٸ� ������ ����
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            // UI ���� ���콺�� �ִٸ� ��¡ ���¸� �����ϰ� ���� ����
            if (isCharging)
            {
                isCharging = false;
                currentChargeTime = 0f;
                HideTrajectory(); // ���� ����
            }
            return;
        }





        // ���� ���õ� ������ Ȯ��
        QuickSlot currentSlot = quickSlotParent.NowSelectedSlot;
        if (currentSlot == null || currentSlot.slotData.item == null)
            return;

        // �����е� ���������� Ȯ��
        if (currentSlot.slotData.item is Item_JumpPad)
        {
            // ���콺 ��Ŭ���� ������ ���� ��� ���
            if (Input.GetMouseButtonDown(0))
            {
                UseJumpPad();
                return; // �����е�� Ư�� ó���ϹǷ� ���⼭ ����
            }
        }
        // �Ϲ� ��ô ������ ó�� (���� �ڵ�)
        else if (currentSlot.slotData.item is Item_Throwing)
        {
            // ���� ��ô ���� ����...
            // ���콺 ��Ŭ���� ������ ���� (��¡ ����)
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                currentChargeTime = 0f;
                ShowTrajectory(); // ���� ǥ��
            }
            // ���콺 ��Ŭ���� ������ �ִ� ���� (��¡ ���� �� ���� ������Ʈ)
            else if (isCharging && Input.GetMouseButton(0))
            {
                currentChargeTime += Time.deltaTime;

                // ���� ��¡�� ������ ���� ������Ʈ
                float currentForce = Mathf.Clamp(minThrowForce + currentChargeTime * chargeRate, minThrowForce, maxThrowForce);
                UpdateTrajectory(currentForce);
            }
            // ���콺 ��Ŭ���� ���� ���� (������)
            else if (isCharging && Input.GetMouseButtonUp(0))
            {
                isCharging = false; // ��¡ ���� ����

                // ���� ���� �� ���
                float finalThrowForce = minThrowForce + currentChargeTime * chargeRate;
                finalThrowForce = Mathf.Clamp(finalThrowForce, minThrowForce, maxThrowForce);

                // ������ ������ �Լ� ȣ��
                ThrowCurrentItem(finalThrowForce);

                currentChargeTime = 0f; // ��¡ �ð� �ʱ�ȭ
                HideTrajectory(); // ���� ����
            }
        }
    }


    // �����е� ��� ��� �޼���
    private void UseJumpPad()
    {
        // ���� ���õ� ���� Ȯ��
        QuickSlot currentSlot = quickSlotParent.NowSelectedSlot;

        // ���Կ� �������� �ְ� Item_JumpPad Ÿ������ Ȯ��
        if (currentSlot != null && currentSlot.slotData.item != null && currentSlot.slotData.item is Item_JumpPad jumpPadItem)
        {
            // �÷��̾� ��ġ ��������
            GameObject player = PlayerManager.Instance.instancePlayer.gameObject;
            Vector3 playerPosition = player.transform.position;

            // �÷��̾� �ٷ� �Ʒ��� �����е� ���� (�ణ �Ʒ��� ������)
            Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y - 0.1f, playerPosition.z);

            // �����е� ������ �ν��Ͻ�ȭ
            GameObject jumpPad = Instantiate(jumpPadItem.instancePrefab, spawnPosition, Quaternion.identity);

            // �����е尡 ���� �ð� �� ��������� ����
            Destroy(jumpPad, 2f);

            // ������ �Һ� ó��
            jumpPadItem.Consume(currentSlot.slotData);

            // ���� UI ������Ʈ
            currentSlot.SlotViewUpdate();
            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
    }


    // ������ ThrowCurrentItem �޼��� - �� �Ű����� �߰�
    public void ThrowCurrentItem(float force = 10f) // �⺻�� ����
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

            // �� �������� �� ���ϱ� (���޹��� force ���)
            rb.AddForce(throwPosition.forward * force, ForceMode.Impulse);

            // ���� �ð� �� ������Ʈ �ı�
            Destroy(thrownObject, destroyDelay);


            // ���� UI ������Ʈ
            currentSlot.SlotViewUpdate();

            PlayerManager.Instance.instancePlayer.Status.inventory.UpdateUI();
        }
    }

    // --- ���� �̸����� ���� �Լ��� ---

    private void ShowTrajectory()
    {
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = true;
        }
    }

    private void HideTrajectory()
    {
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;
        }
    }

    private void UpdateTrajectory(float currentForce)
    {
        if (trajectoryLine == null) return;

        trajectoryLine.positionCount = linePointCount;

        // ���� ��ġ�� ��Ȯ�� ����
        Vector3 startPosition = throwPosition.position;
        // ���� ���� Ȯ�� - ī�޶� �ٶ󺸴� �������� ����
        Vector3 direction = throwPosition.forward;

        Vector3 startVelocity = direction * currentForce;

        // ù ��° ���� ��Ȯ�� ���� ��ġ��
        trajectoryLine.SetPosition(0, startPosition);

        // ������ ������ �ð��� ���� ���
        for (int i = 1; i < linePointCount; i++)
        {
            float time = i * lineTimeStep;
            Vector3 point = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
            trajectoryLine.SetPosition(i, point);
        }
    }
}