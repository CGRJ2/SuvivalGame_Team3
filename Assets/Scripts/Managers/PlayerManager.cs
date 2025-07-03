using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // 플레이어 인스턴스에 전역으로 접근하기 위한 클래스 

    [HideInInspector] public PlayerController instancePlayer;

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
