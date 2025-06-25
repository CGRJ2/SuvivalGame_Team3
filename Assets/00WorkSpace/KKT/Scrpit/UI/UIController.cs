using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Inventory Tabs")]
    public InventoryTabs inventoryTabs;

    // ... �ʿ��ϴٸ� �߰��� �� ����

    // ==== �˸� ���� ====
    private Coroutine notificationRoutine;
    public void ShowLocationNotification(string msg, float duration = 2f)
    {
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
        if (interactionPrompt != null)
            interactionPrompt.SetActive(isShow);
    }

    // ==== �κ��丮/Ŀ�ǵ� On/Off ====
    public void ShowInventory(bool isShow)
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isShow);
        }

        if (isShow)
        {
            Cursor.visible = true;
            Debug.Log("���콺 Ȱ��ȭ");
            
            if(inventoryTabs!=null)
            {
                inventoryTabs.ShowMap();
            }
        }
        else
        {
            Cursor.visible = false;
            Debug.Log("���콺 ��Ȱ��ȭ");
        }
    }
    public void ShowCommand(bool isShow)
    {
        if (commandPanel != null)
            commandPanel.SetActive(isShow);
    }

    // ... �ʿ�� UI���� Show/Hide �Լ� �߰�!
}
