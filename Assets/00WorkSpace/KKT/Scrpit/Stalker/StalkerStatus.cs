using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerStatus : MonoBehaviour
{
    public StateMachine<StalkerStateTypes> state;

    //인벤토리
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float ColliderRange { get; set; }
    [field: SerializeField] public float AttackRange { get; set; }
}
public enum StalkerStateTypes
{
    Idle, Sleep, Move, Chase
}