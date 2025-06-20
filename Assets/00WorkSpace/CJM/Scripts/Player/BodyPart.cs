using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    BodyPartTypes types;
    public ObservableProperty<bool> Activate;
    public ObservableProperty<int> Hp;

    [SerializeField] List<GameObject> ModelsPrefab;

    // �ݶ��̴� �ǰ� ���� �߰�

    public void InitParts()
    {

    }

    public void TakeDamaged(int damage)
    {
        Hp.Value -= damage;
    }
}
