using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftInteract : MonoBehaviour
{
    private bool canInteract = false;
    public UIController UIController;

    public void Update()
    {
        // 상호 작용이 가능하고, 입력이 들어오면
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            // 이동은 가능하고, 마우스 작동만 안되게 하기
            UIController.Instance.ShowInteractionPrompt(false);
            UIController.Instance.ShowCraftPanel(true);
            UIEscape.Instance.OpenPanel(UIController.Instance.craftPanel);
            Debug.Log("스택");
        }

        // 상호 작용이 가능하고, 크래프터 패널이 열려있고, esc를 누르면
        //if (canInteract && Input.GetKeyDown(KeyCode.Escape))
        //{
        //    UIController.Instance.ShowInteractionPrompt(true);
        //    UIController.Instance.ShowCraftPanel(false);
        //}
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract= true;
            UIController.Instance.ShowInteractionPrompt(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract= false;

            UIController.Instance.ShowInteractionPrompt(false);
            UIController.Instance.ShowCraftPanel(false);
        }
    }
}
