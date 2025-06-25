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
        if (alertLevel > 0f)
            alertLevel -= alertDecayRate * Time.deltaTime;

        var newState = EvaluateAlertState();

        if (newState != CurrentState)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownThreshold)
            {
                CurrentState = newState;
                cooldownTimer = 0f;
                OnPerceptionStateChanged?.Invoke(newState);
            }
        }
        else
        {
            cooldownTimer = 0f;
        }
    }

    public void IncreaseAlert(float amount)
    {
        alertLevel += amount;
        alertLevel = Mathf.Clamp(alertLevel, 0, 100);
        Debug.Log($"[Perception] Alert Áõ°¡ ¡æ {alertLevel:F1}");
    }

    private MonsterPerceptionState EvaluateAlertState()
    {
        if (alertLevel >= thresholdHigh) return MonsterPerceptionState.Alert;
        if (alertLevel >= thresholdMedium) return MonsterPerceptionState.Search;
        if (alertLevel >= thresholdLow) return MonsterPerceptionState.Suspicious;
        return MonsterPerceptionState.Idle;
    }

    public float GetAlertLevel() => alertLevel;
}
