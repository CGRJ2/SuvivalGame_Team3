using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField]BodyPartTypes types;
    public ObservableProperty<bool> Activate;
    public ObservableProperty<int> Hp;

    // 콜라이더 피격 판정 추가

    public void InitParts()
    {
        Activate.Value = true;
    }

    public void TakeDamaged(int damage)
    {
        Hp.Value -= damage;
        if (Hp.Value <= 0)
        {
            Activate.Value = false;
        }
    }

    // 트리거로 해야할까? 내적을 통한 피격 판정 공부해오자.
}
