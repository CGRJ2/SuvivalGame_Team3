using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform cameraFocusTransform;
    [SerializeField] private Transform avatar;

    [HideInInspector] public Animator animator;
    [SerializeField] private GameObject FxPrefab;

    Rigidbody rb;
    Vector2 currentRotation;
    public Vector3 moveDir;

    public ColliderController cc;


    public bool isAiming;
    [SerializeField] Vector3 freeCamForward;
    [SerializeField] Vector3 freeCamRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<ColliderController>();
        animator = avatar.GetComponent<Animator>();
    }

    #region 플레이어 이동 결과 관련
    // WASD이동 결과 출력 (플레이어 Position)
    public Vector3 SetMove(Vector3 inputDir, float moveSpeed)
    {
        Vector3 moveDirection = GetMoveDirection(inputDir);

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

        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation,rotateSpeed * Time.deltaTime);
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

        transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);

        Vector3 currentEuler = cameraFocusTransform.localEulerAngles;
        cameraFocusTransform.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }

    public Vector3 GetMoveDirection(Vector2 inputDir)
    {
        Vector3 direction =
           (transform.right * inputDir.x) +
           (transform.forward * inputDir.y);

        return direction.normalized;
    }

    /*public void FreeCamSet(bool isActive)
    {
        if (isActive)
        {
            freeCamActive = true;
            freeCamForward = avatar.transform.forward;
            freeCamRight = avatar.transform.right;
        }
        else
        {
            freeCamForward = cameraFocusTransform.forward;
            freeCamRight = cameraFocusTransform.right;

            // 카메라를 현재 캐릭터 정면으로 옮기기
            freeCamActive = false;
        }

    }*/

    // 플레이어 이동 방향 반환
    /*public Vector3 GetMoveDirection(Vector2 inputDir)
    {
        Vector3 forward;
        Vector3 right;
        // 일반 모드 : 카메라 기준 이동
        if (!freeCamActive)
        {
            forward = cameraFocusTransform.forward;
            right = cameraFocusTransform.right;
            right.y = 0;
            forward.y = 0;
            right.Normalize();
            forward.Normalize();
        }
        // 자유 카메라 모드 : 캐릭터 기준 이동
        else
        {
            forward = freeCamForward;
            right = freeCamRight;
        }
        Vector3 direction =
            // 단위벡터 (1,0,0) * input.x { (-1~1,0,0) => -1~1 }
            (right * inputDir.x) +
            // 단위벡터 (0,0,1) + input.z { (0, 0, -1~1) => -1~1 }
            (forward * inputDir.y);

        return direction.normalized;
    }*/
    #endregion

    #region 상태에 따른 출력
    
    #endregion
}
