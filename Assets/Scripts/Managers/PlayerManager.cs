using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데이터 매니저라고 보면 될듯
public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerController instancePlayer;


    public void Init()
    {
        base.SingletonInit();
    }


    public void PlayerFaint() // => 배터리가 0이 되었을 때 호출
    {
        Debug.Log("플레이어 기절 => 최대 배터리량 감소");
        MoveToLastCamp();
    }

    public void PlayerDead() // => 머리 내구도가 0이 되었을 때 호출
    {
        MoveToLastCamp();
    }

    // 마지막으로 저장된 캠프로 이동
    public void MoveToLastCamp()
    {
        Debug.Log("마지막으로 저장된 캠프로 이동");
    }

}
