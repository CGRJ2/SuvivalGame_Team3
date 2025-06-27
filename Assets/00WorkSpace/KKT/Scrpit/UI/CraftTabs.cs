using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTabs : MonoBehaviour
{
    public UIController UIController;

    [Header("CraftPanels")]
    public GameObject craftPanel;
    public GameObject disassemblePanel;

    public void ShowCraft()
    {
        craftPanel.SetActive(true);
        disassemblePanel.SetActive(false);
    }
    public void ShowDisassemble()
    {
        craftPanel.SetActive(false);
        disassemblePanel.SetActive(true);
    }

    public void CloseCraftWindow()
    {
        UIController.Instance.craftPanel.SetActive(false);
        UIController.Instance.playerInformation.SetActive(true);
        UIController.Instance.quickSlot.SetActive(true);
        UIController.Instance.interactionPrompt.SetActive(true);
    }

}
