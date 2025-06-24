using System.Collections;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    public Rigidbody rigid;
    public GameObject target;
    public Animator anim;
    public StalkerStatus status;
    public float fov = 60;

    public LayerMask targetMask;
    public StateMachine<string> stateMachine;

    private bool isAttack = false;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRadius = 1.0f;

    private readonly int move = Animator.StringToHash("IsMoving");

    protected virtual void Awake()
    {
        status ??= GetComponent<StalkerStatus>();
        anim ??= GetComponent<Animator>();
        rigid ??= GetComponent<Rigidbody>();

        targetMask = (1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        FindTarget();

        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (!isAttack)
        {
            if (distance > attackRadius)
            {
                anim.SetBool(move, true);
                Move();
            }
            else
            {
                anim.SetBool(move, false);
                Attack();
            }
        }
    }

    public void Move()
    {
        rigid.velocity = Vector3.zero;
        
        Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirToTarget), status.RotateSpeed * Time.deltaTime);
        transform.Translate(dirToTarget * status.MoveSpeed * Time.deltaTime, Space.World);
        
    }

    public void Attack()
    {
        Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

        if (!(Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad))) return;

        isAttack = true;
        anim.SetTrigger("Attack");
    }

    private void Attacking()
    {
        Collider[] others = Physics.OverlapSphere(attackTransform.position, attackRadius, targetMask);

        foreach (var other in others)
        {
            if (other.CompareTag("Player"))
            {
                IDamagable target = other.GetComponent<IDamagable>();
                //target.TakeDamage(status.Damage); 스토커는 데미지 안받습니다
                break;
            }
        }
        StartCoroutine(ResetAttackState(1f));
    }

    IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttack = false;
    }

    public void FindTarget()
    {
        if (target != null) return;

        Collider[] targets = Physics.OverlapSphere(transform.position, status.ColliderRange, targetMask);

        if (targets.Length > 0)
        {
            target = targets[0].gameObject;
        }
    }
}