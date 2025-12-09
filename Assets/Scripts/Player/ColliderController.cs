using System.Linq;
using UnityEngine;
public class ColliderController : MonoBehaviour
{
    [SerializeField] Transform avatar;
    public bool IsGroundedSensor;
    public bool IsHeadBlockedSensor;
    CapsuleCollider avatarCollider;
    Vector3 _defaultColiderCenter;   // 앉았을 때 콜라이더 변경한 걸 원복하기 위한 필드
    float _defaultColiderHeight;

    [SerializeField] LayerMask collisionLayerMask;

    [Header("Ground Collision Set")]
    [SerializeField] float rayRadius_Ground;
    [SerializeField] float offsetY_Ground;
    [SerializeField] float distance_Ground;

    [Header("Head Collision Set")]
    [SerializeField] float rayRadius_Head;
    [SerializeField] float offsetY_Head;
    [SerializeField] float distance_Head;

    public IInteractable InteractableObj { get; set; }

    [Header("Crouching Collider Set")]
    [SerializeField] Vector3 crouchColiderCenter;
    [SerializeField] float crouchColiderHeight;

    private void Awake()
    {
        avatarCollider = GetComponent<CapsuleCollider>();
        _defaultColiderCenter = avatarCollider.center;
        _defaultColiderHeight = avatarCollider.height;
    }

    public void GroundCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * offsetY_Ground;
        RaycastHit[] raycastHits = Physics.SphereCastAll(rayOrigin, rayRadius_Ground, Vector3.down, distance_Ground, collisionLayerMask);
        raycastHits = raycastHits.Where(hit => !hit.collider.isTrigger).ToArray();

        IsGroundedSensor = raycastHits.Any();
    }

    public void HeadCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * offsetY_Head;
        RaycastHit[] raycastHits = Physics.SphereCastAll(rayOrigin, rayRadius_Head, Vector3.up, distance_Head, collisionLayerMask);
        raycastHits = raycastHits.Where(hit => !hit.collider.isTrigger).ToArray();

        IsHeadBlockedSensor = raycastHits.Any();
    }

    public void SetColliderCrouch()
    {
        avatarCollider.center = crouchColiderCenter;
        avatarCollider.height = crouchColiderHeight;
    }

    public void SetColliderDefault()
    {
        avatarCollider.center = _defaultColiderCenter;
        avatarCollider.height = _defaultColiderHeight;
    }

    private void OnDrawGizmosSelected()
    {
        /// 그라운드 체크용 Sphere 레이
        // 기본 설정
        Vector3 origin = transform.position + Vector3.up * offsetY_Ground;
        float radius = rayRadius_Ground;
        float maxDistance = distance_Ground;

        // 레이 방향
        Vector3 direction = Vector3.down;

        // 레이 끝 점 계산
        Vector3 endPoint = origin + direction * maxDistance;

        // 색상 설정
        Gizmos.color = Color.yellow;

        // 원통처럼 보이도록 반투명 구체 두 개 + 선 그리기
        Gizmos.DrawWireSphere(origin, radius);
        Gizmos.DrawWireSphere(endPoint, radius);
        Gizmos.DrawLine(origin + Vector3.right * radius, endPoint + Vector3.right * radius);
        Gizmos.DrawLine(origin + Vector3.left * radius, endPoint + Vector3.left * radius);
        Gizmos.DrawLine(origin + Vector3.forward * radius, endPoint + Vector3.forward * radius);
        Gizmos.DrawLine(origin + Vector3.back * radius, endPoint + Vector3.back * radius);
        //////////////////////////////////////////////////////////////////////////////////////

        /// 헤드 체크용 Sphere 레이
        // 기본 설정
        Vector3 originH = transform.position + Vector3.up * offsetY_Head;
        float radiusH = rayRadius_Head;
        float maxDistanceH = distance_Head;

        // 레이 방향
        Vector3 directionH = Vector3.up;

        // 레이 끝 점 계산
        Vector3 endPointH = originH + directionH * maxDistanceH;

        // 색상 설정
        Gizmos.color = Color.yellow;

        // 원통처럼 보이도록 반투명 구체 두 개 + 선 그리기
        Gizmos.DrawWireSphere(originH, radiusH);
        Gizmos.DrawWireSphere(endPointH, radiusH);
        Gizmos.DrawLine(originH + Vector3.right * radiusH, endPointH + Vector3.right * radiusH);
        Gizmos.DrawLine(originH + Vector3.left * radiusH, endPointH + Vector3.left * radiusH);
        Gizmos.DrawLine(originH + Vector3.forward * radiusH, endPointH + Vector3.forward * radiusH);
        Gizmos.DrawLine(originH + Vector3.back * radiusH, endPointH + Vector3.back * radiusH);
        /////////////////////////////////////////////////////////////////////////////////////////

        /*/// 상호작용 범위
        // Gizmos 색상 지정
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // 초록색 투명
        Vector3 origin_Interact = avatar.transform.position + avatar.transform.forward * offset_Interact.z + avatar.transform.up * offset_Interact.y + avatar.transform.right * offset_Interact.x;
        Gizmos.DrawSphere(origin_Interact, rayRadius_Interact);*/
    }

}


