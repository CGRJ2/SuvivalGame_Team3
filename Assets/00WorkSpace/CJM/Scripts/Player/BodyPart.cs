using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    BodyPartTypes types;
    public ObservableProperty<bool> Activate;
    public ObservableProperty<int> Hp;

    [SerializeField] List<GameObject> ModelsPrefab;

    // 콜라이더 피격 판정 추가

    public void InitParts()
    {

    }

    public void TakeDamaged(int damage)
    {
        Hp.Value -= damage;
    }
}
