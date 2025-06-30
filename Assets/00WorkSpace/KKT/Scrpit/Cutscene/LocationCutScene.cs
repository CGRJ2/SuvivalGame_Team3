using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class LocationCutScene : MonoBehaviour
{
    public PlayableDirector timeline;
    public CinemachineVirtualCamera playerVcam;      // �÷��̾� ���󰡴� vCam
    public CinemachineVirtualCamera cutsceneVcam;    // �ƾ��� vCam
    public PlayerInput playerInput;

    public int playerPriority = 20;     // ��� �÷��̾� vCam �켱���� (����)
    public int cutscenePriority = 30;   // �ƾ� vCam �켱���� (�� ����)
    public int normalPriority = 10;     // ��� ��Ȱ��ȭ ��

    private bool played = false;

    void OnTimelineStopped(PlayableDirector pd) // Ÿ�Ӷ��� ������ �ڵ� ȣ��
    {
        timeline.stopped -= OnTimelineStopped;

        // �ƾ� vCam ��Ȱ��ȭ, �÷��̾� vCam Ȱ��ȭ
        cutsceneVcam.Priority = normalPriority;
        playerVcam.Priority = playerPriority;

        // input ����
        if (playerInput != null) playerInput.enabled = true;

        Debug.Log("Timeline ��, �÷��̾� ī�޶�� ����!");
    }

    void OnTriggerEnter(Collider other) // Player�� Trigger�ȿ� ������ �ƾ� ����
    {
        if (!played && other.CompareTag("Player"))

            Debug.Log("Timeline ����, �ƾ� ī�޶�� ����!");
        {
            played = true;

            // input ����
            if (playerInput != null) playerInput.enabled= false;

            // �ƾ� vCam Ȱ��ȭ, �÷��̾� vCam ��Ȱ��ȭ(Priority�� ����)
            cutsceneVcam.Priority = cutscenePriority;
            playerVcam.Priority = normalPriority;

            // timeline ���
            timeline.Play();
            timeline.stopped += OnTimelineStopped;
        }
    }
}
