using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
    // ������ �ٸ� �Ŵ����麸�� �� �������� �ʱ�ȭ �ؾ� ��
    PlayerManager pm;
    public void Init()
    {
        base.SingletonInit();
        pm = PlayerManager.Instance;
    }
}
