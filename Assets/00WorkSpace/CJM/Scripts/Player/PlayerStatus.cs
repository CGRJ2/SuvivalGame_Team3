using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //public StateMachine<PlayerStateTypes> state = new StateMachine<PlayerStateTypes>();
    public StateMachine<PlayerMovementStateTypes> movementState = new StateMachine<PlayerMovementStateTypes>();



    public ObservableProperty<int> WillPower;
    public ObservableProperty<int> Battery;
    private Dictionary<BodyPartTypes, BodyPart> bodyParts;

    //private Inventory inventory;
    
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }


    public void InitPlayerData()
    {

    }

    public void SavePlayerData()
    {

    }
}
public enum PlayerMovementStateTypes
{
    Idle, Move, Sprint, Jump, Fall, Crouch // 구르기 추가 필요 
}

public enum PlayerStateTypes
{
    Idle, Attack, Damaged, Dead //Exhausted,
}
public enum BodyPartTypes
{
    Head, Body, LeftArm, RightArm, LeftLeg, RightLeg
}