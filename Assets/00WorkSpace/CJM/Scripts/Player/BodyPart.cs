using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField]BodyPartTypes types;
    public ObservableProperty<bool> Activate;
    public ObservableProperty<int> Hp;

    // �ݶ��̴� �ǰ� ���� �߰�

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

    // Ʈ���ŷ� �ؾ��ұ�? ������ ���� �ǰ� ���� �����ؿ���.
}
