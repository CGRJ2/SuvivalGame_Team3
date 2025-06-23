using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampLevel : MonoBehaviour
{
    private int CampNowLevel = 0;// 현재 베이스캠프의레벨
    private int CampMaxLevel = 3; // 베이스캠프 최대레벨

    void Start()
    {
        CampNowLevel = 0;
        Debug.Log("현재 레벨: " + CampNowLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))//임시 레벨업 기능
        {
            CampLevelUp();
        }
    }

    
    public void CampLevelUp()//레벨상승기능
    {

        if (CampNowLevel >= CampMaxLevel)//최대레벨 도달 했는지 체크
        {
            Debug.Log("베이스캠프가 최대 레벨입니다");
            return;
        }
        CampNowLevel++;
        Debug.Log("베이스캠프 레벨업 현재 레벨: " + CampNowLevel);
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