using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamagable : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} 공격 받음");
    }
}
