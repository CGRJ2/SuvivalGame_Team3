using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // 플레이어 인스턴스에 전역으로 접근하기 위한 클래스 

    public PlayerController instancePlayer;


    public void Init()
    {
        base.SingletonInit();
    }

    // => 배터리가 0이 되었을 때 호출
    public void PlayerFaint() 
    {
        Debug.Log("플레이어 기절 => 최대 배터리량 감소");
        MoveToLastCamp();
    }

    // => 머리 내구도가 0이 되었을 때 호출
    public void PlayerDead() 
    {
        Debug.Log("플레이어 사망!");
        MoveToLastCamp();
    }

    // 마지막으로 저장된 캠프로 이동
    public void MoveToLastCamp()
    {
        Debug.Log("마지막으로 저장된 캠프로 이동");
    }

}
