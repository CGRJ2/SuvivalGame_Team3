using Cinemachine;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam; // 씬 안에 있는 TPS 카메라 인스펙터 상에서 연결
    private PlayerInputReader _input;
    private CinemachineTransposer _transposer;

    [Header("Perspective Settings")]
    [SerializeField] private float yawSpeed = 150f;
    [SerializeField] private float pitchSpeed = 120f;
    [SerializeField] private float minPitch = -40f;
    [SerializeField] private float maxPitch = 70f;

    private float _yaw;
    private float _pitch;

    [Header("Zoom Settings")]
    [SerializeField] float[] zoomDistances = { 2.0f, 3.5f, 6.0f, 8.0f };
    int maxDistanceIndex => zoomDistances.Length - 1;
    int currentDistanceIndex = 1; // 시작 카메라 인덱스 (줌 레벨 : 3.5f)

    // 부드러운 전환용
    [SerializeField] private float zoomSmoothTime = 0.15f; // 0.1~0.2 정도 추천

    private Vector3 _followOffset;
    private float _curDistance;
    private float _targetDistance;   // 목표 거리 (휠 입력으로만 변경)
    private float _zoomVelocity;     // SmoothDamp용 속도 저장 값

    private void Awake() => Init();

    private void LateUpdate()
    {
        HandleSightRotation();
        HandleZoom();
    }

    void Init()
    {
        _input = GetComponentInParent<PlayerInputReader>();
        // 화면 이동 설정 필드 초기화
        var euler = transform.rotation.eulerAngles;
        _yaw = euler.y;
        _pitch = euler.x;


        // 초기 줌 레벨 세팅
        _transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();

        _curDistance = zoomDistances[currentDistanceIndex];
        _targetDistance = _curDistance;

        _followOffset = _transposer.m_FollowOffset;
        _followOffset.z = -_curDistance;
        _transposer.m_FollowOffset = _followOffset;
    }

    void HandleSightRotation() // 시점 회전 처리
    {
        Vector2 look = _input.Data.Rotate;

        _yaw += look.x * yawSpeed * Time.deltaTime;
        _pitch -= look.y * pitchSpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);



        // ===이전 로직===
        // 사이드 캠 활성화 상태에선 화면회전은 정지
        //if (CameraManager.Instance.activeSideView)
        //{
        //    View.SetAvatarRotation(View.facingDir, pm.RotateSpeed);
        //    return;
        //}

        //Vector3 camRotateDir = SetAimRotation(MouseInputDir, pm.MinPitch, pm.MaxPitch);

        //Vector3 avatarDir;

        //// 프리캠 모드 => 플레이어의 이동 방향으로 아바타의 방향 맞춰주기
        //if (isFreeCamModInput) avatarDir = View.facingDir;
        //// 제 자리에 멈춰서서 프리캠 모드가 아니라면, 공격 도중이라면 =>  아바타가 플레이어의 화면을 향해 응시
        //else if (!isMoveInput || IsCurrentState(PlayerStateTypes.Attack)) avatarDir = camRotateDir;
        //else avatarDir = View.moveDir;



        //// 컨트롤 락 걸리면 아바타 회전은 정지
        //if (Model.isControllLocked) return;
        //View.SetAvatarRotation(avatarDir, pm.RotateSpeed);
    }

    void HandleZoom() // Zoom 처리
    {
        // 1. 현재 프레임 휠 입력 읽기
        float scroll = _input.Data.ZoomDelta;
        _input.Data.ZoomDelta = 0f;

        // 2. 스크롤이 입력이 있으면 => "목표 인덱스" 변경
        if (Mathf.Abs(scroll) > 0.01f)
        {
            if (scroll < 0f)
            {
                if (currentDistanceIndex < maxDistanceIndex)
                    currentDistanceIndex++;
            }
            else
            {
                if (currentDistanceIndex > 0)
                    currentDistanceIndex--;
            }
            
            // 새 [목표 거리]만 갱신
            _targetDistance = zoomDistances[currentDistanceIndex];
        }

        // 3. 매 프레임 부드럽게 [현재 거리 -> 목표 거리] 로 이동
        _curDistance = Mathf.SmoothDamp(
            _curDistance,
            _targetDistance,
            ref _zoomVelocity,
            zoomSmoothTime
        );

        _followOffset.z = -_curDistance;
        _transposer.m_FollowOffset = _followOffset;
    }
}
