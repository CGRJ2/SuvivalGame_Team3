using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public StateMachine<PlayerStateTypes> state;

    public ObservableProperty<int> WillPower;
    public ObservableProperty<int> Battery;
    private Dictionary<BodyPartTypes, BodyPart> bodyParts;

    //인벤토리
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }

}
public enum PlayerStateTypes
{
    Idle, Move, Run, Exhausted, Attack, Damaged, Dead
}
public enum BodyPartTypes
{
    Head, leftArm, rightArm, leftLeg, rightLeg
}