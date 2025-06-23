using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampLevel : MonoBehaviour
{
    private int CampNowLevel = 0;// ���� ���̽�ķ���Ƿ���
    private int CampMaxLevel = 3; // ���̽�ķ�� �ִ뷹��

    void Start()
    {
        CampNowLevel = 0;
        Debug.Log("���� ����: " + CampNowLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))//�ӽ� ������ ���
        {
            CampLevelUp();
        }
    }

    
    public void CampLevelUp()//������±��
    {

        if (CampNowLevel >= CampMaxLevel)//�ִ뷹�� ���� �ߴ��� üũ
        {
            Debug.Log("���̽�ķ���� �ִ� �����Դϴ�");
            return;
        }
        CampNowLevel++;
        Debug.Log("���̽�ķ�� ������ ���� ����: " + CampNowLevel);
    }

    public int GetCampNowLevel()
    {
        return CampNowLevel;
    }

    public bool IsCampMaxLevel()
    {
        return CampNowLevel >= CampMaxLevel;
    }
}