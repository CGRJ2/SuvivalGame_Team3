using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField]private bool isGrounded;
    [SerializeField]private bool isHeadTouched;
    CapsuleCollider movementCollid;
    Vector3 defaultCenter_MC;
    float defaultHeight_MC;
    
    [SerializeField] LayerMask collisionLayerMask;

    [Header("Ground Collision Set")]
    [SerializeField] float rayRadius_Ground;
    [SerializeField] float offsetY_Ground;
    [SerializeField] float distance_Ground;

    [Header("Head Collision Set")]
    [SerializeField] float rayRadius_Head;
    [SerializeField] float offsetY_Head;
    [SerializeField] float distance_Head;

    [Header("Crouching Collider Set")]
    [SerializeField] Vector3 crouchCenter_MC;
    [SerializeField] float crouchHeight_MC;

    private void Awake()
    {
        movementCollid = GetComponent<CapsuleCollider>();
        defaultCenter_MC = movementCollid.center;
        defaultHeight_MC = movementCollid.height;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        HeadCheck();
    }

    public void GroundCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * offsetY_Ground;
        RaycastHit[] raycastHits = Physics.SphereCastAll(rayOrigin, rayRadius_Ground, Vector3.down, distance_Ground, collisionLayerMask);

        if (raycastHits.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HeadCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * offsetY_Head;
        RaycastHit[] raycastHits = Physics.SphereCastAll(rayOrigin, rayRadius_Head, Vector3.up, distance_Head, collisionLayerMask);

        if (raycastHits.Length > 0)
        {
            isHeadTouched = true;
        }
        else
        {
            isHeadTouched = false;
        }
    }

    public void SetColliderCrouch()
    {
        movementCollid.center = crouchCenter_MC;
        movementCollid.height = crouchHeight_MC;
    }

    public void SetColliderDefault()
    {
        movementCollid.center = defaultCenter_MC;
        movementCollid.height = defaultHeight_MC;
    }

    public bool GetIsGroundState()
    {
        return isGrounded;
    }

    // 머리 위에 막고 있으면 false. Crouch 도중 일어날 수 있는지 없는지를 판단하는 함수
    public bool GetIsHeadTouchedState()
    {
        return isHeadTouched;
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
    }

}
