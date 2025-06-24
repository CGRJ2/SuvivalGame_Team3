using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool isNowActive = UIController.Instance.commandPanel.activeSelf;
            UIController.Instance.ShowCommand(!isNowActive);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isNowActive = UIController.Instance.inventoryPanel.activeSelf;
            UIController.Instance.ShowInventory(!isNowActive);
        }
    }
}
