using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Payload : MonoBehaviour
{
    public Transform[] checkPoints; // 경유지점
    public float moveSpeed; // 화물 이동 속도
    public float playerDetectRadius; // 플레이어 인식 범위
    public GameObject player; // 플레이어

    private int curCheckPointIndex = 0;
    private bool isMoving = false; // 이동 상태 확인
    private bool atEnd = false; // 도착 여부 확인
    private bool isUnlocked = false; // 조건 확인

    private void Start()
    {
        transform.position = checkPoints[0].position;
    }
    private void Update()
    {
        if (atEnd) return;
        if (player == null) return;

        // 플레이어가 인식 범위 안에 있는지
        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool isPlayerNear = dist <= playerDetectRadius;

        // 저장을 안 했고, 이동 시작 전, 플레이어가 범위 안에 존재, 상호 작용을 했을 경우
        if (!isUnlocked && !isMoving && isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("저장 안해서 상호작용 불가능");
        }

        // 저장을 했고, 이동 시작 전, 플레이어가 범위 안에 존재, 상호 작용을 했을 경우
        if (isUnlocked &&!isMoving && isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("저장 완료! 비행기 이동");
            isMoving = true;
        }

        // 이동 시작 후, 플레이어가 범위 안 존재
        if (isMoving && isPlayerNear)
        {
            Vector3 target = checkPoints[curCheckPointIndex].position;
            Vector3 dir = (target - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            // 도착 체크
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                transform.position = target;
                curCheckPointIndex++;
                if (curCheckPointIndex >= checkPoints.Length)
                {
                    atEnd = true;
                    isMoving = false;
                    OnPayloadArrived();
                }
            }
        }
    }
    // 플레이어 사망시 재시작
    public void ResetToStart()
    {
        atEnd = false;
        curCheckPointIndex = 0;
        isMoving = false;
        transform.position = checkPoints[0].position;
    }
    // 도착 지점에 도착
    private void OnPayloadArrived()
    {
        Debug.Log("화물 도착");
        SceneManager.LoadScene("EmptyTestScene");
    }
    // 저장 했을 시 조건 해제
    public void UnlockPayload()
    {
        isUnlocked = true;
    }
}
