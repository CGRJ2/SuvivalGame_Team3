using System.Collections.Generic;
using UnityEngine;
public class ColliderController : MonoBehaviour
{
    [SerializeField] Transform avatar;
    private bool isGrounded;
    private bool isHeadTouched;
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

    [Header("Attack Range Set")]
    [SerializeField] LayerMask attackableLayerMask;
    [SerializeField] float rayRadius_Attack;
    [SerializeField] Vector3 offset_Attack;
    IDamagable[] damagablesInRange;


    /*[SerializeField] LayerMask interactableLayerMask;
    [SerializeField] float rayRadius_Interact;
    [SerializeField] Vector3 offset_Interact;
    IInteractable[] interactablesInRange;*/
    public IInteractable InteractableObj { get; set; }

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
        AttackRangeCheck();
        //InteractRangeCheck();
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

    public void AttackRangeCheck()
    {
        Vector3 origin = avatar.transform.position + avatar.transform.forward * offset_Attack.z + avatar.transform.up * offset_Attack.y + avatar.transform.right * offset_Attack.x;

        Collider[] cols = Physics.OverlapSphere(origin, rayRadius_Attack, attackableLayerMask);
        List<IDamagable> damagables = new List<IDamagable>();

        foreach (Collider col in cols)
        {
            damagables.Add(col.GetComponent<IDamagable>());
        }
        this.damagablesInRange = damagables.ToArray();
    }
    /*public void InteractRangeCheck()
    {
        Vector3 origin = avatar.transform.position + avatar.transform.forward * offset_Interact.z + avatar.transform.up * offset_Interact.y + avatar.transform.right * offset_Interact.x;

        Collider[] cols = Physics.OverlapSphere(origin, rayRadius_Interact, interactableLayerMask);
        List<IInteractable> interactables = new List<IInteractable>();

        foreach (Collider col in cols)
        {
            interactables.Add(col.GetComponent<IInteractable>());
        }
        this.interactablesInRange = interactables.ToArray();
    }*/

    public IDamagable[] GetDamagablesInRange()
    {
        return damagablesInRange;
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

    // �Ӹ� ���� ���� ������ false. Crouch ���� �Ͼ �� �ִ��� �������� �Ǵ��ϴ� �Լ�
    public bool GetIsHeadTouchedState()
    {
        return isHeadTouched;
    }

    private void OnDrawGizmosSelected()
    {
        /// �׶��� üũ�� Sphere ����
        // �⺻ ����
        Vector3 origin = transform.position + Vector3.up * offsetY_Ground;
        float radius = rayRadius_Ground;
        float maxDistance = distance_Ground;

        // ���� ����
        Vector3 direction = Vector3.down;

        // ���� �� �� ���
        Vector3 endPoint = origin + direction * maxDistance;

        // ���� ����
        Gizmos.color = Color.yellow;

        // ����ó�� ���̵��� ������ ��ü �� �� + �� �׸���
        Gizmos.DrawWireSphere(origin, radius);
        Gizmos.DrawWireSphere(endPoint, radius);
        Gizmos.DrawLine(origin + Vector3.right * radius, endPoint + Vector3.right * radius);
        Gizmos.DrawLine(origin + Vector3.left * radius, endPoint + Vector3.left * radius);
        Gizmos.DrawLine(origin + Vector3.forward * radius, endPoint + Vector3.forward * radius);
        Gizmos.DrawLine(origin + Vector3.back * radius, endPoint + Vector3.back * radius);
        //////////////////////////////////////////////////////////////////////////////////////

        /// ��� üũ�� Sphere ����
        // �⺻ ����
        Vector3 originH = transform.position + Vector3.up * offsetY_Head;
        float radiusH = rayRadius_Head;
        float maxDistanceH = distance_Head;

        // ���� ����
        Vector3 directionH = Vector3.up;

        // ���� �� �� ���
        Vector3 endPointH = originH + directionH * maxDistanceH;

        // ���� ����
        Gizmos.color = Color.yellow;

        // ����ó�� ���̵��� ������ ��ü �� �� + �� �׸���
        Gizmos.DrawWireSphere(originH, radiusH);
        Gizmos.DrawWireSphere(endPointH, radiusH);
        Gizmos.DrawLine(originH + Vector3.right * radiusH, endPointH + Vector3.right * radiusH);
        Gizmos.DrawLine(originH + Vector3.left * radiusH, endPointH + Vector3.left * radiusH);
        Gizmos.DrawLine(originH + Vector3.forward * radiusH, endPointH + Vector3.forward * radiusH);
        Gizmos.DrawLine(originH + Vector3.back * radiusH, endPointH + Vector3.back * radiusH);
        /////////////////////////////////////////////////////////////////////////////////////////


        /// ���� ����
        // Gizmos ���� ����
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // ������ ����
        Vector3 origin_Attack = avatar.transform.position + avatar.transform.forward * offset_Attack.z + avatar.transform.up * offset_Attack.y + avatar.transform.right * offset_Attack.x;
        Gizmos.DrawSphere(origin_Attack, rayRadius_Attack);


        /*/// ��ȣ�ۿ� ����
        // Gizmos ���� ����
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // �ʷϻ� ����
        Vector3 origin_Interact = avatar.transform.position + avatar.transform.forward * offset_Interact.z + avatar.transform.up * offset_Interact.y + avatar.transform.right * offset_Interact.x;
        Gizmos.DrawSphere(origin_Interact, rayRadius_Interact);*/
    }

}
