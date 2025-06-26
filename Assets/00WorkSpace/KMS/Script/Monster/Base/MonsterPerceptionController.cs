using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterPerceptionController
{
    private readonly BaseMonster owner;

    private float alertLevel = 0f;
    private float alertDecayRate;
    private float thresholdLow;
    private float thresholdMedium;
    private float thresholdHigh;

    private float cooldownTimer = 0f;
    private float cooldownThreshold;
    private float perceptionInterval = 1f;

    public MonsterPerceptionState CurrentState { get; private set; } = MonsterPerceptionState.Idle;

    public event Action<MonsterPerceptionState> OnPerceptionStateChanged;

    public MonsterPerceptionController(BaseMonster owner, float decayRate, float cooldownThreshold,
        float low, float medium, float high)
    {
        this.owner = owner;
        this.alertDecayRate = decayRate;
        this.cooldownThreshold = cooldownThreshold;
        this.thresholdLow = low;
        this.thresholdMedium = medium;
        this.thresholdHigh = high;
    }

    public void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= perceptionInterval)
        {
            cooldownTimer = 0f;

            if (owner.checkTargetVisible)
            {
                IncreaseAlert(10f); // 감지되었을 경우 Alert 상승
            }
            else
            {
                DecreaseAlert(); // 시간이 지나면 Alert 감소
            }

            EvaluatePerceptionState(); // 상태 전이 판단
        }
    }

    public void IncreaseAlert(float amount)
    {
        alertLevel = Mathf.Min(100f, alertLevel + amount);
    }

    private void DecreaseAlert()
    {
        alertLevel = Mathf.Max(0f, alertLevel - alertDecayRate);
    }

    private void EvaluatePerceptionState()
    {
        MonsterPerceptionState newState = owner.GetCurrentPerceptionState();

        if (alertLevel >= owner.AlertThreshold_High)
            newState = MonsterPerceptionState.Alert;
        else if (alertLevel >= owner.AlertThreshold_Medium)
            newState = MonsterPerceptionState.Search;
        else if (alertLevel >= owner.AlertThreshold_Low)
            newState = MonsterPerceptionState.Suspicious;
        else
            newState = MonsterPerceptionState.Idle;

        if (newState != CurrentState)
        {
            CurrentState = newState;
            OnPerceptionStateChanged?.Invoke(newState);
        }
    }

    public void ForceSetState(MonsterPerceptionState newState)
    {
        CurrentState = newState;
        OnPerceptionStateChanged?.Invoke(newState);
    }

    public float GetAlertLevel() => alertLevel;
}
