using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // 플레이어 인스턴스에 전역으로 접근하기 위한 클래스 

    [HideInInspector] public PlayerController instancePlayer;
    DataManager dm;


    [Header("런타임 내 불변 값 세팅")] /////////////////////
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    public float MinPitch { get { return minPitch; } }
    public float MaxPitch { get { return maxPitch; } }
    [SerializeField] private float rotateSpeed_Init;
    [SerializeField] private float crouchSpeed_Init;
    [field: SerializeField] public float DamagedInvincibleTime { get; private set; }
    [field: SerializeField] public float AttackCoolTime { get; private set; }
    public float CrouchSpeed { get { return crouchSpeed_Init; } }
    public float RotateSpeed { get { return rotateSpeed_Init; } }
    //////////////////////////////////////////////////////

    [Header("초기값 세팅")]////////////////////////
    [SerializeField] public float moveSpeed_Init;
    [SerializeField] public float sprintSpeed_Init;
    [SerializeField] public float jumpForce_Init;
    [SerializeField] public int damage_Init;
    [SerializeField][Range(0.1f, 2)] public float mouseSensitivity_Init;
    




    public void Init()
    {
        base.SingletonInit();
        dm = DataManager.Instance;
    }


    // => 배터리가 0이 되었을 때 호출
    public void PlayerFaint(SaveDataGroup saveDataGroup) 
    {
        Debug.Log("다음날 오전 9시로 / 맵 내의 몬스터 정보 초기화 / 맵 내 파밍 오브젝트 정보 초기화");

        // 다음날 오전 9시로 이동
        DailyManager.Instance.currentTimeData.CurrentDay.Value += 1;
        DailyManager.Instance.currentTimeData.CurrentTime = 0;
        DailyManager.Instance.currentTimeData.TZ_State.Value = TimeZoneState.Morning;

        // 배터리 최대량 감소
        instancePlayer.Status.Init_AfterFaint(); 

        // 위치 이동
        MoveToLastCamp();

        // 자동 저장
        //dm.SaveData(0);
    }

    // => 머리 내구도가 0이 되었을 때 호출
    public void PlayerDead() 
    {
        //마지막 세이브 데이터로 이동(날짜, 시간 / 맵 내의 몬스터 & 파밍 오브젝트 정보 초기화 / 플레이어 정보 초기화)
        dm.LoadData(0);

        // 부위별 최대 내구도 깎고 시작
        instancePlayer.Status.Init_AfterDead(); 

        // 위치 이동
        MoveToLastCamp();

        // 자동 저장
        dm.SaveData(0);

    }

    // 마지막으로 저장된 캠프로 이동
    public void MoveToLastCamp()
    {
        BaseCampManager bcm = BaseCampManager.Instance;
        Debug.Log("마지막으로 저장된 캠프로 이동");

        // 간이 캠프 데이터가 있다면 
        if (bcm.tempCampData != null)
        {
            Debug.Log("간이캠프로 이동");
            instancePlayer.Respawn(bcm.tempCampData.respawnPoint);

            // 인스턴스가 있다면 
            if (bcm.currentTempCampInstance != null)
            {
                // 간이캠프 소모(파괴)
                bcm.currentTempCampInstance.DestroyTempCamp();
            }
            // 인스턴스가 없다면 
            else
            {
                // 현재 리스폰 포인트(이전에 저장된 간이 캠프 위치)에 간이 캠프 프리펩 소환
                GameObject tempCampInstance = bcm.SpawnTempBaseCampInstance(bcm.tempCampData.respawnPoint);
                TemporaryCampInstance temp = tempCampInstance.GetComponent<TemporaryCampInstance>();
                
                // 간이캠프 소모(파괴)
                if (temp != null) temp.DestroyTempCamp();
            }

            // 간이 캠프에서 리스폰 되면서 간이캠프 제거
            bcm.tempCampData = null;
        }
        // 없으면 베이스캠프로 이동
        else
        {
            Debug.Log("베이스캠프로 이동");
            instancePlayer.Respawn(bcm.baseCampInstance.GetRespawnPointTransform());
        }
    }

}
