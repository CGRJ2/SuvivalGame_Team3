using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform cameraFocusTransform;
    [SerializeField] private Transform avatar;

    [HideInInspector] public Animator animator;
    [SerializeField] private GameObject FxPrefab;

    Rigidbody rb;
    Vector2 currentRotation;
    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 facingDir;


    Vector3 freeCamForward;
    Vector3 freeCamRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = avatar.GetComponent<Animator>();
    }

    #region 플레이어 이동 결과 관련
    // WASD이동 결과 출력 (플레이어 Position)
    public Vector3 SetMove(Vector3 getMoveDir, float moveSpeed)
    {
        Vector3 moveDirection = getMoveDir;

        Vector3 velocity = rb.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        rb.velocity = velocity;

        return moveDirection;
    }

    public void Jump(float jumpForce)
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // WASD이동 결과 출력 (아바타 방향 맞추기)
    public void SetAvatarRotation(Vector3 direction, float rotateSpeed)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    // 마우스 이동 결과 출력 (카메라 origin 회전)
    public Vector3 SetAimRotation(Vector2 MouseDirection, float _minPitch, float _maxPitch)
    {
        // Vector2 inputDir = GetMouseDirection();

        // // X방향 회전은 각도 제한 없음.
        currentRotation.x += MouseDirection.x;

        currentRotation.y = Mathf.Clamp(
            currentRotation.y + MouseDirection.y,
            _minPitch,
            _maxPitch
            );

        cameraFocusTransform.rotation = Quaternion.Euler(0, currentRotation.x, 0);

        // y 회전 제한
        Vector3 currentEuler = cameraFocusTransform.localEulerAngles;
        cameraFocusTransform.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

        Vector3 rotateDirVector = cameraFocusTransform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }

    public Vector3 GetMoveDirection(Vector2 inputDir)
    {
        Vector3 right = cameraFocusTransform.right;
        Vector3 forward = cameraFocusTransform.forward;
        right.y = 0;
        forward.y = 0;

        Vector3 direction =
           (right.normalized * inputDir.x) +
           (forward.normalized * inputDir.y);

        return direction.normalized;
    }
    public Vector3 GetMoveDirection(Vector2 inputDir, bool freeCamMod)
    {
        if (freeCamMod)
        {
            Vector3 direction =
            (freeCamRight * inputDir.x) +
            (freeCamForward * inputDir.y);
            return direction.normalized;
        }
        else return Vector3.zero;
    }
    public void FreeCamSet(bool isActive)
    {
        freeCamForward = cameraFocusTransform.forward;
        freeCamForward.y = 0;
        freeCamForward.Normalize();
        freeCamRight = cameraFocusTransform.right;
        freeCamRight.y = 0;
        freeCamRight.Normalize();
        
    }

    // 플레이어 이동 방향 반환

    #endregion

    #region 상태에 따른 출력

    #endregion
}
