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

    #region �÷��̾� �̵� ��� ����
    // WASD�̵� ��� ��� (�÷��̾� Position)
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

    // WASD�̵� ��� ��� (�ƹ�Ÿ ���� ���߱�)
    public void SetAvatarRotation(Vector3 direction, float rotateSpeed)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation,rotateSpeed * Time.deltaTime);
    }

    // ���콺 �̵� ��� ��� (ī�޶� origin ȸ��)
    public Vector3 SetAimRotation(Vector2 MouseDirection, float _minPitch, float _maxPitch)
    {
        // Vector2 inputDir = GetMouseDirection();

        // // X���� ȸ���� ���� ���� ����.
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

            // ī�޶� ���� ĳ���� �������� �ű��
            freeCamActive = false;
        }

    }*/

    // �÷��̾� �̵� ���� ��ȯ
    /*public Vector3 GetMoveDirection(Vector2 inputDir)
    {
        Vector3 forward;
        Vector3 right;
        // �Ϲ� ��� : ī�޶� ���� �̵�
        if (!freeCamActive)
        {
            forward = cameraFocusTransform.forward;
            right = cameraFocusTransform.right;
            right.y = 0;
            forward.y = 0;
            right.Normalize();
            forward.Normalize();
        }
        // ���� ī�޶� ��� : ĳ���� ���� �̵�
        else
        {
            forward = freeCamForward;
            right = freeCamRight;
        }
        Vector3 direction =
            // �������� (1,0,0) * input.x { (-1~1,0,0) => -1~1 }
            (right * inputDir.x) +
            // �������� (0,0,1) + input.z { (0, 0, -1~1) => -1~1 }
            (forward * inputDir.y);

        return direction.normalized;
    }*/
    #endregion

    #region ���¿� ���� ���
    
    #endregion
}
