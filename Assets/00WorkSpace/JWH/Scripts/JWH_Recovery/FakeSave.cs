using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSave : MonoBehaviour
{
    public void NotSave()
    {
        Debug.Log("���̺� �Ϸ�! (������ ������� ����)");
    }

    public void NotLoad()
    {
        Debug.Log("�ε� �Ϸ�! (������ �ҷ����� ����)");
    }
}
