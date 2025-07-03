using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // 플레이어 인스턴스에 전역으로 접근하기 위한 클래스 

    [HideInInspector] public PlayerController instancePlayer;


    public void Init()
    {
        base.SingletonInit();
    }

    public void SavePlayerData()
    {
        // 아래 2가지 데이터 저장
        // PlayerStatus status = instancePlayer.Status;
        // Transform playerTranform = instancePlayer.transform;
    }

    public void LoadPlayerData()
    {
        // 아래 2가지 데이터 불러오기
        // PlayerStatus status = instancePlayer.Status;
        // Transform playerTranform = instancePlayer.transform;
    }


    // => 배터리가 0이 되었을 때 호출
    public void PlayerFaint() 
    {
        // 배터리 최대량 감소
        instancePlayer.Status.Init_AfterFaint(); 
        Debug.Log("다음날 오전 9시로 / 맵 내의 몬스터 정보 초기화 / 맵 내 파밍 오브젝트 정보 초기화");

        // 위치 이동
        MoveToLastCamp();
    }

    // => 머리 내구도가 0이 되었을 때 호출
    public void PlayerDead() 
    {
        // 부위별 최대 내구도 깎고 시작
        instancePlayer.Status.Init_AfterDead(); 

        Debug.Log("마지막 세이브 데이터로 이동(날짜, 시간 / 맵 내의 몬스터 정보 초기화 / 맵 내 파밍 오브젝트 정보 초기화 / 자동저장)");

        // 위치 이동
        MoveToLastCamp();
    }

    // 마지막으로 저장된 캠프로 이동
    public void MoveToLastCamp()
    {
        BaseCampManager bcm = BaseCampManager.Instance;
        Debug.Log("마지막으로 저장된 캠프로 이동");

        // 간이 캠프가 있다면
        if (bcm.tempCampData != null)
        {
            Debug.Log("간이캠프로 이동");
            Debug.Log(bcm.tempCampData);
            instancePlayer.Respawn(bcm.tempCampData.respawnPoint);

            // 간이 캠프에서 리스폰 되면서 간이캠프 제거
            bcm.currentTempCampInstance.DestroyTempCamp();
        }
        // 없으면 베이스캠프로 이동
        else
        {
            Debug.Log("베이스캠프로 이동");
            instancePlayer.Respawn(bcm.baseCampInstance.GetRespawnPointTransform());
        }
    }

}
