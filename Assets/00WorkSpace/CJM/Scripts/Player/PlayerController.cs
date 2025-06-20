using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerViewer view;
    public Vector3 InputDir { get; private set; }
    public Vector3 MouseDir { get; private set; }
    private Vector2 currentRotation;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    [SerializeField][Range(0.1f, 2)] private float mouseSensitivity = 1;

    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private void Update()
    {
        view.Move(GetMoveDirection(), status.MoveSpeed);
        view.SetAvatarRotation(GetMoveDirection(), status.RotateSpeed);
        view.SetAimRotation(MouseDir, minPitch, maxPitch);
    }

    private void InputActionsInit()
    {
        // �÷��̾� ���� ��
        var playerControlMap = GetComponent<PlayerInput>().actions.FindActionMap("PlayerActions");

        // �޸��� �׼�
        runAction = playerControlMap.FindAction("Sprint");
        runAction.Enable();
        runAction.performed += HandleSprint;
        runAction.canceled += HandleSprint;

        // ���� �׼�
        attackAction = playerControlMap.FindAction("Attack");
        attackAction.Enable();
        attackAction.started += HandleAttack;
        attackAction.canceled += HandleAttack;

        // ���� �׼�
        jumpAction = playerControlMap.FindAction("Jump");
        jumpAction.Enable();
        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJump;

        
    }

    

    

    public Vector3 GetMoveDirection()
    {
        Vector3 direction =
            // �������� (1,0,0) * input.x { (-1~1,0,0) => -1~1 }
            (transform.right * InputDir.x) +
            // �������� (0,0,1) + input.z { (0, 0, -1~1) => -1~1 }
            (transform.forward * InputDir.y);

        return direction.normalized;
    }


    public void OnMove(InputValue value)
    {
        InputDir = value.Get<Vector2>();
    }
    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseDir = mouseDir * mouseSensitivity;
    }


    public void HandleSprint(InputAction.CallbackContext context)
    {
        /*if (context.performed)
            isSprintInput = true;
        else if (context.canceled)
            isSprintInput = false;*/
    }
    public void HandleJump(InputAction.CallbackContext context)
    {
        /*if (context.performed)
        {
            isJumpCut = false;
            isJumpInput = true;
        }
        if (context.canceled)
        {
            isJumpCut = true;
            isJumpInput = false;
        }*/
    }

    public void HandleAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            /*AttackDir = mouseWorldPos - (Vector2)transform.position;
            // ���� ����(&& Fall���� ����ó��) => �Ϲ� ����
            if (colliderState.isGrounded && stateMachine.CurState != stateMachine.stateDic[PlayerStateTypes.Fall])
            {
                if (stateMachine.CurState is Player_Attack attackState)
                {
                    attackState.AttackPlayByIndex();
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.stateDic[PlayerStateTypes.Attack]);
                }

            }*/
            // ���� ���� => ���� ����
            /*if (isJumping)
            {

            }*/
        }
    }
}
