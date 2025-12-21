using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 세이브 & 로드 가능
public class PlayerModel
{
    public PlayerLocomotionModel    Locomotion;     // 플레이어 이동 관련 데이터 및 규칙
    public PlayerCombatModel        Combat;         // 플레이어 전투 관련 데이터 및 규칙
    public PlayerSurvivalModel      Survival;       // 플레이어 생존 관련 데이터 및 규칙

    [HideInInspector] public InventoryPresenter inventory;  // 인벤토리 데이터

    public bool IsControllLocked; // 조작 가능/불가 상태

    public bool IsDead;
    public event Action OnDied;

    public bool IsFaint;
    public event Action OnFaint;

    // 상태가 별도로 있지만 조건 체크를 위한 플래그
    public bool IsGrounded;
    public bool IsRolling;

    // 상태와 함께 쓰일 플래그 (별도 상태 없음)
    public bool IsCrouching;
    public bool IsLanding;

    public bool IsInvincible;

    // 플레이어 데이터 초기 상태
    public void Init()
    {
        Locomotion  ??= new PlayerLocomotionModel();
        Combat      ??= new PlayerCombatModel();
        Survival    ??= new PlayerSurvivalModel();

        Locomotion.Init();
        Combat.Init();
        Survival.Init();

        // 인벤토리 초기화
        inventory = new InventoryPresenter();
    }

    // 플레이어 죽고 리스폰 할 때 초기화
    public void DeadRespawn()
    {
        // 정신력, 배터리만 최대로 맞춰주기
        Survival.CurrentWillPower.Value = Manager.data.CapacityTable.FindByKey("Will").Max;
        Survival.InitBattery();

        // 감소한 최대 체력으로 설정
        Survival.BodyPartsInitInRespawn();
    }

    // 플레이어 기절 후 리스폰 할 때 초기화
    public void FaintRespawn()
    {
        var batteryFixedData = Manager.data.CapacityTable.FindByKey("Battery");
        var reduceAmount = batteryFixedData.ReducePerTick;
        var min = batteryFixedData.Min;

        // 최대 배터리 감소
        if (Survival.MaxBattery.Value - reduceAmount > min)
            Survival.MaxBattery.Value -= reduceAmount;
        else
            Survival.MaxBattery.Value = min;

        Survival.CurrentBattery.Value = Survival.MaxBattery.Value;
    }

    public void Tick(float tickDuration)
    {
        if (IsDead || IsFaint) return;
        if (Survival.CurrentBattery.Value > 0)
        {
            var idleBatteryConsume = Manager.data.BatteryConsumeTable.FindByKey("Idle");
            if (Survival.CurrentBattery.Value - idleBatteryConsume.Amount > 0f)
            {
                Survival.CurrentBattery.Value -= idleBatteryConsume.Amount;
            }
            else
            {
                Survival.CurrentBattery.Value = 0f;
                Faint();
            }
        }
    }

    //  부위 랜덤 데미지
    public void TakeDamage(float damage)
    {
        // 죽음 상태라면 데미지 계산 실행X
        if (IsDead) return;

        // 활성 상태인 신체 부위 중 랜덤 선택 -> 이것도 피격 부위 데미지 주는걸로 만들어볼까...
        List<BodyPart> activeBodyPart = new List<BodyPart>();

        // 활성 상태인 파츠들로 리스트 새로 생성 (이미 파괴된 부위를 제외하기 위함)
        var bodyPartsDic = Survival.GetBodyPartsDic();
        foreach (var kvp in bodyPartsDic)
        {
            if (kvp.Value.Activate.Value)
                activeBodyPart.Add(kvp.Value);
        }

        // 활성 상태 부위 랜덤 데미지
        if (activeBodyPart.Count > 0)
        {
            int r = UnityEngine.Random.Range(1, activeBodyPart.Count);
            activeBodyPart[r].TakeDamage(damage);
        }

        // 전체 체력 or 머리 체력이 0 이하면  사망
        if (Survival.SumCurrentHP.Value <= 0 || bodyPartsDic[BodyPartType.Head].Hp.Value <= 0)
            Die();
    }

    public void Die()   // Controller에서 이벤트에 Dead상태 전환 연결해서 처리
    {
        if (IsDead) return;
        IsDead = true;
        OnDied?.Invoke();
    }

    // => 배터리가 0이 되었을 때 호출
    public void Faint() // Controller에서 이벤트에 Faint상태 전환 연결해서 처리
    {
        if (IsFaint) return;
        IsFaint = true;
        OnFaint?.Invoke();
    }
}