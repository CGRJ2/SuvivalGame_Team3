using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class InteractCunScene : MonoBehaviour
{
    [Header("Core")]
    public PlayableDirector timeline;
    public CinemachineVirtualCamera playerVcam;
    public CinemachineVirtualCamera cutsceneVcam;
    public PlayerInput playerInput; // (Input System ���ٸ� Inspector���� ����)

    [Header("Priorty)")]
    public int playerPriority = 20;
    public int cutscenePriority = 30;
    public int normalPriority = 10;

    private bool canInteract = false; // �÷��̾ ���� �ȿ� �ִ���
    private bool played = false; 

    void Update()
    {
        canInteract = false;

        // �÷��̾ ���� �ȿ� �ְ� �ƾ��� ���� ��� ���̸� E�� ������ ��
        if (canInteract && !played && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            played = true;
            
            UIController.Instance.ShowInteractionPrompt(false);

            // �Է� ����
            if (playerInput != null) 
            {
                playerInput.enabled = false;
            }

            // �ƾ� vCam Ȱ��ȭ, �÷��̾� vCam ��Ȱ��ȭ(Priority�� ����)
            cutsceneVcam.Priority = cutscenePriority;
            playerVcam.Priority = normalPriority;

            // timeline ���
            timeline.Play();
            timeline.stopped += OnTimelineStopped;

            Debug.Log("��ȣ�ۿ� �ƾ� ����!");
        }
    }

    void OnTimelineStopped(PlayableDirector pd) // Ÿ�Ӷ��� ������ �ڵ� ȣ��
    {
        timeline.stopped -= OnTimelineStopped;

        // �ƾ� vCam ��Ȱ��ȭ, �÷��̾� vCam Ȱ��ȭ
        cutsceneVcam.Priority = normalPriority;
        playerVcam.Priority = playerPriority;

        // input ����
        if (playerInput != null)
        { 
            playerInput.enabled = false; 
        }

        Debug.Log("��ȣ�ۿ� �ƾ� ��! �Է� ����");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            UIController.Instance.ShowInteractionPrompt(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            UIController.Instance.ShowInteractionPrompt(false);
        }
    }
}
