using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMS.Monster.Interface;

public class CatAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var staggerable = other.GetComponent<IStaggerable>();
        if (staggerable != null)
        {
            staggerable.Stagger();
            Debug.Log("[CatHitbox] 기절 성공");
        }
    }

    // 애니메이션 이벤트에서 호출
    public void EnableHitbox()
    {
        gameObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        gameObject.SetActive(false);
    }
}
