using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class LocationCutScene : MonoBehaviour
{
    public PlayableDirector timeline;
    public CinemachineVirtualCamera playerVcam;      // 플레이어 따라가는 vCam
    public CinemachineVirtualCamera cutsceneVcam;    // 컷씬용 vCam
    public PlayerInput playerInput;

    public int playerPriority = 20;     // 평소 플레이어 vCam 우선순위 (높음)
    public int cutscenePriority = 30;   // 컷씬 vCam 우선순위 (더 높음)
    public int normalPriority = 10;     // 평소 비활성화 값

    private bool played = false;

    void OnTimelineStopped(PlayableDirector pd) // 타임라인 끝나고 자동 호출
    {
        timeline.stopped -= OnTimelineStopped;

        // 컷씬 vCam 비활성화, 플레이어 vCam 활성화
        cutsceneVcam.Priority = normalPriority;
        playerVcam.Priority = playerPriority;

        // input 복구
        if (playerInput != null) playerInput.enabled = true;

        Debug.Log("Timeline 끝, 플레이어 카메라로 복귀!");
    }

    void OnTriggerEnter(Collider other) // Player가 Trigger안에 들어오면 컷씬 시작
    {
        if (!played && other.CompareTag("Player"))

            Debug.Log("Timeline 시작, 컷씬 카메라로 변경!");
        {
            played = true;

            // input 차단
            if (playerInput != null) playerInput.enabled= false;

            // 컷씬 vCam 활성화, 플레이어 vCam 비활성화(Priority로 조정)
            cutsceneVcam.Priority = cutscenePriority;
            playerVcam.Priority = normalPriority;

            // timeline 재생
            timeline.Play();
            timeline.stopped += OnTimelineStopped;
        }
    }
}
