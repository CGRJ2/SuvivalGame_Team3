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
        // ��ȣ �ۿ��� �����ϰ�, �Է��� ������
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            // �̵��� �����ϰ�, ���콺 �۵��� �ȵǰ� �ϱ�
            UIController.Instance.ShowInteractionPrompt(false);
            UIController.Instance.ShowCraftPanel(true);
            UIEscape.Instance.OpenPanel(UIController.Instance.craftPanel);
            Debug.Log("����");
        }

        // ��ȣ �ۿ��� �����ϰ�, ũ������ �г��� �����ְ�, esc�� ������
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
