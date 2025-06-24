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
    public PlayerInput playerInput; // (Input System 쓴다면 Inspector에서 연결)

    [Header("Priorty)")]
    public int playerPriority = 20;
    public int cutscenePriority = 30;
    public int normalPriority = 10;

    private bool canInteract = false; // 플레이어가 범위 안에 있는지
    private bool played = false; 

    void Update()
    {
        canInteract = false;

        // 플레이어가 범위 안에 있고 컷씬이 아직 재생 전이며 E를 눌렀을 때
        if (canInteract && !played && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            played = true;
            
            UIController.Instance.ShowInteractionPrompt(false);

            // 입력 차단
            if (playerInput != null) 
            {
                playerInput.enabled = false;
            }

            // 컷씬 vCam 활성화, 플레이어 vCam 비활성화(Priority로 조정)
            cutsceneVcam.Priority = cutscenePriority;
            playerVcam.Priority = normalPriority;

            // timeline 재생
            timeline.Play();
            timeline.stopped += OnTimelineStopped;

            Debug.Log("상호작용 컷씬 시작!");
        }
    }

    void OnTimelineStopped(PlayableDirector pd) // 타임라인 끝나고 자동 호출
    {
        timeline.stopped -= OnTimelineStopped;

        // 컷씬 vCam 비활성화, 플레이어 vCam 활성화
        cutsceneVcam.Priority = normalPriority;
        playerVcam.Priority = playerPriority;

        // input 복구
        if (playerInput != null)
        { 
            playerInput.enabled = false; 
        }

        Debug.Log("상호작용 컷씬 끝! 입력 복구");
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
