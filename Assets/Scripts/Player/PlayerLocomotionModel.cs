using UnityEngine;

public class PlayerLocomotionModel
{
    [field: SerializeField] public float WalkSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }
    [field: SerializeField] public float CrouchSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }

    
    [Header("설정된 마우스 감도(아직 구현x)")]
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity;
    public float MouseSensitivity { get { return mouseSensitivity; } private set { mouseSensitivity = value; } }

    public void Init()
    {
        var initStatTable = Manager.data.PlayerStatsTable;
        WalkSpeed = initStatTable.FindByKey("MoveSpeed").InitValue;
        SprintSpeed = initStatTable.FindByKey("SprintSpeed").InitValue;
        RotateSpeed = initStatTable.FindByKey("RotateSpeed").InitValue;
        CrouchSpeed = initStatTable.FindByKey("CrouchSpeed").InitValue;
        JumpForce = initStatTable.FindByKey("JumpForce").InitValue;
    }
}
