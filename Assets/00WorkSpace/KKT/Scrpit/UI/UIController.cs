using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ==== UI ������Ʈ ���� ====
    [Header("Player Information")]
    public GameObject playerInformation;

    [Header("Time information")]
    public GameObject timeInformation;

    [Header("Quick Slot")]
    public GameObject quickSlot;

    [Header("Location Notification")]
    public GameObject locationPanel;
    public TextMeshProUGUI locationText;

    [Header("Collect Notification")]
    public GameObject collectPanel;
    public TextMeshProUGUI collectText;

    [Header("System Notification")]
    public GameObject systemPanel;
    public TextMeshProUGUI systemText;

    [Header("Interaction Prompt")]
    public GameObject interactionPrompt;

    [Header("Inventory Panel")]
    public GameObject inventoryPanel;

    [Header("Command Panel")]
    public GameObject commandPanel;

    [Header("Craft Panel")]
    public GameObject craftPanel;

    [Header("Inventory Tabs")]
    public InventoryTabs inventoryTabs;

    // ... �ʿ��ϴٸ� �߰��� �� ����

    // ==== �˸� ���� ====
    private Coroutine notificationCoroutine;
    public void ShowLocationNotification(string msg, float duration = 2f)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        StartCoroutine(ShowAndHide(locationPanel, locationText, msg, duration));
    }
    public void ShowCollectNotification(string msg, float duration = 2f)
    {
        StartCoroutine(ShowAndHide(collectPanel, collectText, msg, duration));
    }
    public void ShowSystemNotification(string msg, float duration = 2f)
    {
        StartCoroutine(ShowAndHide(systemPanel, systemText, msg, duration));
    }
    private IEnumerator ShowAndHide(GameObject panel, TextMeshProUGUI text, string msg, float duration)
    {
        text.text = msg;
        panel.SetActive(true);
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }

    // ==== ��ȣ�ۿ� �ȳ� On/Off ====
    public void ShowInteractionPrompt(bool isShow)
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(isShow);
    }

    // ==== �κ��丮 On/Off ====
    public void ShowInventory(bool isShow)
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(isShow);

        if (isShow)
        {
            Cursor.visible = true;
            if(inventoryTabs!=null) inventoryTabs.ShowItem();
        }
        else Cursor.visible = false;
    }

    // ==== Ŀ�ǵ� On/Off ====
    public void ShowCommand(bool isShow)
    {
        if (commandPanel != null) commandPanel.SetActive(isShow);
    }

    // ==== ũ����Ʈ On/Off ====
    public void ShowCraftPanel(bool isShow)
    {
        if(craftPanel != null) craftPanel.SetActive(isShow);

        if (isShow)
        {
            Cursor.visible = true;
            Debug.Log("���콺 Ȱ��ȭ");

            if (playerInformation != null) playerInformation.SetActive(false);
            if (quickSlot != null) quickSlot.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            Debug.Log("���콺 ��Ȱ��ȭ");

            if (playerInformation != null) playerInformation.SetActive(true);
            if (quickSlot != null) quickSlot.SetActive(true);

            
        }
    }
    public void CloseCraftWindow(bool isShow)
    {
        if (isShow) craftPanel.SetActive(false);
    }

    // ���� ��ġ Ȯ���� ���� �׽�Ʈ �ڵ�
    public TextMeshProUGUI locationNowText;
    public void UpdateCurrentLocation(string locName)
    {
        locationNowText.text = locName;
    }

    // ... �ʿ�� UI���� Show/Hide �Լ� �߰�!
}
