using UnityEngine;
using TMPro;

public class DroppedItem : MonoBehaviour
{
    [Header("Core")]
    public string itemName = "����";          // Inspector���� ���� ����
    public GameObject nameUIPrefab;          // Inspector���� ������ ����
    public float showDistance = 3f;          // UI ǥ�� ����
    public float pickupMessageTime = 2f;     // ���� �޼��� ���ӽð�

    private GameObject nameUIObj;            // �ν��Ͻ�ȭ�� UI ������Ʈ
    private TextMeshProUGUI nameText;        // UI �ؽ�Ʈ
    private Transform player;                // �÷��̾� Transform

    private bool canPickup = false;          // �÷��̾ ���� ���� �ִ���
    private bool picked = false;             // �����ߴ���


    void Start()
    {
        // Canvas ã��
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            //Debug.LogError("Canvas�� ���� �����ϴ�!");
            enabled = false; return;
        }

        // �̸� UI  ����(ó���� ��Ȱ��ȭ)
        nameUIObj = Instantiate(nameUIPrefab, canvas.transform);
        nameText = nameUIObj.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null)
        {
            nameText.text = itemName;
        }
        nameUIObj.SetActive(false);

        // Player ã��
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            //Debug.LogError("Player �±װ� ���� ������Ʈ�� �����ϴ�!");
            enabled = false; return;
        }
        player = playerObj.transform;
    }

    void Update()
    {
        // Null üũ (��� �ڵ�)
        if (player == null || nameUIObj == null) return;

        // �Ÿ� ���
        float distance = Vector3.Distance(transform.position, player.position);
        //Debug.Log($"[DroppedItem] distance={distance}, showDistance={showDistance}");

        // �Ÿ� �̳��� �̸� UI ǥ��, �ƴϸ� ����
        if (distance <= showDistance)
        {
            nameUIObj.SetActive(true);

            // ������ �� ��¦ ����� ǥ��
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.0f);
            nameUIObj.transform.position = screenPos;
            canPickup = true;

            // ��ȣ�ۿ����� ����
            if (Input.GetKeyDown(KeyCode.F))
            {
                Pickup();
            }
        }
        else
        {
            nameUIObj.SetActive(false);
            canPickup = false;
        }
    }
    void Pickup()
    {
        if (picked) return; // �ߺ� ����

        picked = true;

        UIController.Instance.ShowCollectNotification($"{itemName}��(��) ȹ���߽��ϴ�!", pickupMessageTime);

        Destroy(gameObject);
        Destroy(nameUIObj);
    }
}
