using System.Collections;
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
    public float CrouchSpeed { get { return crouchSpeed_Init; } }
    public float RotateSpeed { get { return rotateSpeed_Init; } }
    //////////////////////////////////////////////////////

    [Header("초기값 세팅")]////////////////////////
    [SerializeField] public float moveSpeed_Init;
    [SerializeField] public float sprintSpeed_Init;
    [SerializeField] public float jumpForce_Init;
    [SerializeField] public float knockBackForce_Init;
    [SerializeField] public float damage_Init;
    [SerializeField][Range(0.1f, 2)] public float mouseSensitivity_Init;



    public void Init()
    {
        base.SingletonInit();
        dm = DataManager.Instance;
    }

    private void OnDestroy()
    {

    }

    public void SaveInBaseCamp()
    {
        StartCoroutine(SaveAndWaitUntilNextDayOpen());

    }

    private IEnumerator SaveAndWaitUntilNextDayOpen()
    {
        // 죽음 애니메이션 시작
        // 죽음 애니메이션 종료 후 페이드 아웃
        Panel_FadeInOut dayOffSavePanel = UIManager.Instance.popUpUIGroup.dayOffSavePanel;
        dayOffSavePanel.PopMessage_FadeInOut();

        instancePlayer.Status.isControllLocked = true;

        // 페이드 인이 시작될 즈음까지 대기
        yield return new WaitUntil(() => dayOffSavePanel.isOpenStandBy);

        instancePlayer.Status.isControllLocked = false;

        // 다음날 오전 9시로 이동
        DailyManager.Instance.currentTimeData.CurrentDay.Value += 1;
        DailyManager.Instance.currentTimeData.CurrentTime = 0;
        DailyManager.Instance.currentTimeData.TZ_State.Value = TimeZoneState.Morning;

        // 몬스터 & 파밍오브젝트 초기화
        StageManager.Instance.InitSpawnerRoutines();

        // 자동 저장
        dm.SaveData(0);
    }


    // => 배터리가 0이 되었을 때 호출
    public void PlayerFaint(float currentBattery)
    {
        if (instancePlayer.IsCurrentState(PlayerStateTypes.Dead)) return;
        if (currentBattery > 0) return;
        instancePlayer.stateMachine.ChangeState(instancePlayer.stateMachine.stateDic[PlayerStateTypes.Dead]);
        StartCoroutine(WaitUntilFaintPanelClose());
    }
    private IEnumerator WaitUntilFaintPanelClose()
    {
        // 죽음 애니메이션 시작
        // 죽음 애니메이션 종료 후 페이드 아웃
        Panel_FadeInOut faintPanel = UIManager.Instance.popUpUIGroup.faintPanel;
        faintPanel.PopMessage_FadeInOut();

        // 페이드 인이 시작될 즈음까지 대기
        yield return new WaitUntil(() => faintPanel.isOpenStandBy);

        SetGameAfterFaint();
    }

    public void SetGameAfterFaint()
    {
        // 다음날 오전 9시로 이동
        DailyManager.Instance.currentTimeData.CurrentDay.Value += 1;
        DailyManager.Instance.currentTimeData.CurrentTime = 0;
        DailyManager.Instance.currentTimeData.TZ_State.Value = TimeZoneState.Morning;

        // 배터리 최대량 감소
        instancePlayer.Status.Init_AfterFaint();

        // 일반 상태로 전환
        instancePlayer.stateMachine.ChangeState(instancePlayer.stateMachine.stateDic[PlayerStateTypes.Idle]);

        // 몬스터 & 파밍오브젝트 초기화
        StageManager.Instance.InitSpawnerRoutines();

        // 위치 이동
        BaseCampManager.Instance.MoveToLastCamp(false);

        // 자동 저장
        dm.SaveData(0);
    }


    // => 머리 내구도가 0이 되었을 때 호출
    public void PlayerDead()
    {
        instancePlayer.stateMachine.ChangeState(instancePlayer.stateMachine.stateDic[PlayerStateTypes.Dead]);
        StartCoroutine(WaitUntilDeadPanelClose());
    }

    private IEnumerator WaitUntilDeadPanelClose()
    {
        // 죽음 애니메이션 시작
        // 죽음 애니메이션 종료 후 페이드 아웃
        Panel_FadeInOut deadPanel = UIManager.Instance.popUpUIGroup.deadPanel;
        deadPanel.PopMessage_FadeInOut();

        // 페이드 인이 시작될 즈음까지 대기
        yield return new WaitUntil(() => deadPanel.isOpenStandBy);

        // 마지막 세이브 데이터로 이동(날짜, 시간 / 맵 내의 몬스터 & 파밍 오브젝트 정보 초기화 / 플레이어 정보 초기화), 위치 이동
        dm.LoadData(0);

        // 부위별 최대 내구도 깎고 시작
        instancePlayer.Status.Init_AfterDead();

        // 일반 상태로 전환
        instancePlayer.stateMachine.ChangeState(instancePlayer.stateMachine.stateDic[PlayerStateTypes.Idle]);

        // 최근 임시텐트 있다면 파괴
        if (BaseCampManager.Instance.currentTempCampInstance != null)
            BaseCampManager.Instance.currentTempCampInstance.DestroyWithData();

        // 자동 저장
        dm.SaveData(0);

    }


}
