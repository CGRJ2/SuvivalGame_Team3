using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public bool isGrounded;
    [SerializeField] float groundRayRadius;
    [SerializeField] float Yoffset;
    [SerializeField] float distance;
    [SerializeField] LayerMask collisionLayerMask;

    private void FixedUpdate()
    {
        GroundCheck();
    }

    public void GroundCheck()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * Yoffset;
        RaycastHit[] raycastHits = Physics.SphereCastAll(rayOrigin, groundRayRadius, Vector3.down, distance, collisionLayerMask);
        
        if (raycastHits.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        /// 그라운드 체크용 Sphere 레이
        // 기본 설정
        Vector3 origin = transform.position + Vector3.up * Yoffset;
        float radius = groundRayRadius;
        float maxDistance = distance;

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
    }

}
