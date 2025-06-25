using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] UIManager uiManager;

    // ���� ���� : ��� ��� > ��� ���� > ��� ���� > ���� > ��� ��� ... ////// +�Ͻ�����
    private void Awake() => Init();

    private void Init()
    {
        base.SingletonInit();
        InitalizeOrderSetting();
    }

    // ���ӸŴ��� ������ �̱��� ��ü�� �ʱ�ȭ ���� ����
    private void InitalizeOrderSetting()
    {
        // �ʱ�ȭ�� ������� ����
        uiManager.Init();
    }

}
