using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> state;

    public ObservableProperty<int> WillPower;
    public ObservableProperty<int> Battery;
    private Dictionary<BodyPartTypes, BodyPart> bodyParts;

    //private Inventory inventory;
    
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }


    public void InitPlayerData()
    {

    }

    public void SavePlayerData()
    {

    }
}
public enum PlayerStateTypes
{
    Idle, Move, Run, Exhausted, Attack, Damaged, Dead
}
public enum BodyPartTypes
{
    Head, Body, LeftArm, RightArm, LeftLeg, RightLeg
}