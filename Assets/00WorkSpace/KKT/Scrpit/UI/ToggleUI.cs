using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
    public GameObject targetUI;
    public InputAction toggleAction;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (targetUI != null)
            {
                targetUI.SetActive(!targetUI.activeSelf);
            }
        }
    }
}
