using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSave : MonoBehaviour
{
    public void TryNotSave()
    {
        if (GimmickManager.CanSaveNow())
        {
            Debug.Log("���̺�! (������ ������� ����)");
        }
        else
        {
            Debug.LogWarning("���� �ڸ� �� �׾�~");
        }
    }

    public void NotLoad()
    {
        Debug.Log("�ε�! (������ �ҷ����� ����)");
    }
}
