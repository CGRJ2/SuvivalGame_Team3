using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    //[SerializeField] private Transform aim;
    [SerializeField] private Transform avatar;
    [SerializeField] private Transform cameraFocusTransform;
    
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject FxPrefab;

    Rigidbody rb;
    Vector2 currentRotation;
    public bool freeCamActive;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #region 플레이어 입력 결과 출력
    // WASD이동 결과 출력 (플레이어 Position)
    public Vector3 Move(Vector3 dir, float moveSpeed)
    {
        Vector3 moveDirection = dir;

        Vector3 velocity = rb.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        rb.velocity = velocity;
        return moveDirection;
    }

    // WASD이동 결과 출력 (아바타 방향 맞추기)
    public void SetAvatarRotation(Vector3 dir, float rotateSpeed)
    {
        if (dir == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    // 마우스 이동 결과 출력 (카메라 origin 회전)
    public Vector3 SetAimRotation(Vector3 mouseDir, float minPitch, float maxPitch)
    {
        currentRotation.x += mouseDir.x;
        currentRotation.y = Mathf.Clamp(currentRotation.y + mouseDir.y, minPitch, maxPitch);

        cameraFocusTransform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

        //Vector3 currentEuler = aim.localEulerAngles;
        //aim.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

        Vector3 rotateDirVector = cameraFocusTransform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }

    // 카메라 기준 이동 방향 설정
    public Vector3 GetMoveDirection(Vector2 inputDir)
    {
        Vector3 forward;
        Vector3 right;
        // 우클릭 중이라면 카메라 기준 이동, 아니라면 카메라 자유 이동
        if (!freeCamActive)
        {
            forward = cameraFocusTransform.forward;
            right = cameraFocusTransform.right;
            right.y = 0;
            forward.y = 0;
            right.Normalize();
            forward.Normalize();
        }
        else
        {
            forward = transform.forward;
            right = transform.right;
        }
        Vector3 direction =
            // 단위벡터 (1,0,0) * input.x { (-1~1,0,0) => -1~1 }
            (right * inputDir.x) +
            // 단위벡터 (0,0,1) + input.z { (0, 0, -1~1) => -1~1 }
            (forward * inputDir.y);

        return direction.normalized;
    }
    #endregion

    #region 상태에 따른 출력
    public void StateMachineInit(StateMachine<PlayerStateTypes> state)
    {
        state.stateDic.Add(PlayerStateTypes.Idle, new Player_Idle(this));
        state.stateDic.Add(PlayerStateTypes.Move, new Player_Move(this));
        state.stateDic.Add(PlayerStateTypes.Run, new Player_Run(this));
    }
    #endregion
}
