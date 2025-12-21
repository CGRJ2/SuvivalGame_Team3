using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurvivalModel : IDisposable
{
    [Header("플레이어 생존 수치 정보")]
    public ObservableProperty<float> CurrentWillPower = new ObservableProperty<float>();
    public ObservableProperty<float> CurrentBattery = new ObservableProperty<float>();
    public ObservableProperty<float> MaxBattery = new ObservableProperty<float>();
    public ObservableProperty<float> SumCurrentHP = new ObservableProperty<float>();
    public ObservableProperty<float> SumCurrentMaxHP = new ObservableProperty<float>();
    
    private Dictionary<BodyPartType, BodyPart> _bodyPartsDic = new();

    public void Init()
    {
        // 정신력 초기화
        CurrentWillPower.Value = Manager.data.CapacityTable.FindByKey("Will").Max;

        // 배터리 초기화
        InitBattery();

        // 신체 부위 초기화
        BodyPartsInit();
    }

    // 신체 부위별 인스턴스 생성
    public void BodyPartsInit()
    {
        Panel_PlayerStatus playerStatusUI = Manager.ui.inventoryGroup.panel_PlayerStatus;

        var bodyStatsTable = Manager.data.BodyStatsTable;
        string bodyKey = "";

        for (int i = 0; i < Enum.GetValues(typeof(BodyPartType)).Length; i++)
        {
            switch ((BodyPartType)i)
            {
                case BodyPartType.Head:
                    bodyKey = "HeadHP";
                    break;

                case BodyPartType.LeftArm:
                case BodyPartType.RightArm:
                    bodyKey = "ArmHP";
                    break;

                case BodyPartType.LeftLeg:
                case BodyPartType.RightLeg:
                    bodyKey = "LegHP";
                    break;

                default:
                    bodyKey = "";
                    break;
            }

            if (string.IsNullOrWhiteSpace(bodyKey))
            {
                Debug.LogError($"테이블에 존재하지 않는 신체부위 타입 존재: {(BodyPartType)i}");
                break;
            }

            var bodyPartData = bodyStatsTable.FindByKey(bodyKey);
            var reduceAmount = bodyPartData.ReduceAmount;
            var maxHP = bodyPartData.Max;
            var minHp = bodyPartData.Min;

            BodyPart bodyPart = new BodyPart((BodyPartType)i, maxHP);
            _bodyPartsDic[(BodyPartType)i] = bodyPart;
        }
    }

    // 사망 후 신체부위 최대 내구도 감소
    public void BodyPartsInitInRespawn()
    {
        var bodyStatsTable = Manager.data.BodyStatsTable;
        string bodyKey = "";

        foreach (var kvp in _bodyPartsDic)
        {
            switch (kvp.Key)
            {
                case BodyPartType.Head:
                    bodyKey = "HeadHP";
                    break;

                case BodyPartType.LeftArm:
                case BodyPartType.RightArm:
                    bodyKey = "ArmHP";
                    break;

                case BodyPartType.LeftLeg:
                case BodyPartType.RightLeg:
                    bodyKey = "LegHP";
                    break;
            }

            var bodyPartData = bodyStatsTable.FindByKey(bodyKey);
            var reduceAmount = bodyPartData.ReduceAmount;
            var minHp = bodyPartData.Min;

            if (_bodyPartsDic[kvp.Key].CurrentMaxHp.Value - reduceAmount > minHp)
                _bodyPartsDic[kvp.Key].CurrentMaxHp.Value -= reduceAmount;
            else
                _bodyPartsDic[kvp.Key].CurrentMaxHp.Value = minHp;

            _bodyPartsDic[kvp.Key].Hp = _bodyPartsDic[kvp.Key].CurrentMaxHp;
        }
    }

    public void InitBattery()
    {
        MaxBattery.Value = Manager.data.CapacityTable.FindByKey("Battery").Max;
        CurrentBattery.Value = MaxBattery.Value;
    }

    public void ChargeBattery(float amount)
    {
        if (CurrentBattery.Value + amount < MaxBattery.Value)
            CurrentBattery.Value += amount;
        else CurrentBattery.Value = MaxBattery.Value;
    }

    public Dictionary<BodyPartType, BodyPart> GetBodyPartsDic()
    {
        return _bodyPartsDic;
    }

    // 모든 부위의 현재 체력의 합을 Model 필드의 SumCurrentHP로 환산해주는 함수
    public void CalculateCurrentHPSum(float hp)
    {
        float sumHP = 0;
        foreach (var kvp in _bodyPartsDic)
        {
            sumHP += kvp.Value.Hp.Value;
        }
        SumCurrentHP.Value = sumHP;
    }

    // 모든 부위의 최대 체력의 합을 Model 필드의 SumCurrentMaxHP로 환산해주는 함수
    public void CalculateCurrentMaxHPSum(float hp)
    {
        float sumMaxHP = 0;
        foreach (var kvp in _bodyPartsDic)
        {
            sumMaxHP += kvp.Value.CurrentMaxHp.Value;
        }
        SumCurrentMaxHP.Value = sumMaxHP;
    }
    public void Dispose()
    {
        CurrentWillPower.UnsubscribeAll();
        CurrentBattery.UnsubscribeAll();
        MaxBattery.UnsubscribeAll();
        SumCurrentHP.UnsubscribeAll();
        SumCurrentMaxHP.UnsubscribeAll();
    }
}
public enum BodyPartType
{
    Head, LeftArm, RightArm, LeftLeg, RightLeg
}