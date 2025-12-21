using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] Collider _collider;

    Transform _owner;
    float _damage;
    float _knockback;
    float _hitStun;
    float _iFrame;

    bool _isActive;

    readonly HashSet<IDamageable> _alreadyHit = new HashSet<IDamageable>(); // 공격 중복 판정을 막기 위함

    void Awake()
    {
        _collider ??= GetComponent<Collider>();
        _collider.isTrigger = true;
        _collider.enabled = false;
        _isActive = false;
    }

    public void Init(Transform owner)
    {
        _owner = owner;
    }

    public void Configure(float damage, float knockback, float hitStun, float iFrame)
    {
        _damage = damage;
        _knockback = knockback;
        _hitStun = hitStun;
        _iFrame = iFrame;
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        _collider.enabled = active;

        // 새 스윙 시작 => 맞춘 대상 초기화
        if (active)
            _alreadyHit.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isActive) return;
        if (other.isTrigger) return;
        if (other.transform == _owner) return;

        var damageable = other.GetComponentInParent<IDamageable>();
        if (damageable == null) return;
        if (_alreadyHit.Contains(damageable)) return;

        _alreadyHit.Add(damageable);

        var hitPoint = other.ClosestPoint(transform.position);
        var hitNormal = (hitPoint - transform.position).normalized;

        var hit = new HitInfo
        {
            Attacker = _owner,
            HitPoint = hitPoint,
            HitNormal = hitNormal,
            Damage = _damage,
            KnockbackPower = _knockback,
            HitStun = _hitStun,   // 피격 애니메이션 주기에 맞춰서 변경
            IFrame = _iFrame
        };

        damageable.TakeDamage(hit);
    }
}
