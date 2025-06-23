using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialStateMachine stateMachine;

    // ���� Ʃ�丮�� UI ������Ʈ ����
    public GameObject moveGuideUI, jumpGuideUI, runGuideUI, attackGuideUI, basecampGuidUI, saveGuidUI, productionGuidUI, equipmentGuidUI, saveGuideUI;

    void Awake()
    {
        stateMachine = new TutorialStateMachine();
        stateMachine.AddState(TutorialStateType.Move, new TutorialMoveState(this));
        stateMachine.AddState(TutorialStateType.Jump, new TutorialJumpState(this));
        stateMachine.AddState(TutorialStateType.Run, new TutorialRunState(this));
        stateMachine.AddState(TutorialStateType.Attack, new TutorialAttackState(this));
        stateMachine.AddState(TutorialStateType.Jump, new TutorialProductionState(this));
    }

    void Start()
    {
        stateMachine.ChangeState(TutorialStateType.Move); // Ʃ�丮�� ����
    }

    void Update()
    {
        stateMachine.Update();
    }

    // �÷��̾� �ൿ ���� �޼��� ����
    public bool PlayerDidMove() { /* ���� �̵� ���� �ڵ� */ return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0; }
    public bool PlayerDidJump() { /* ���� ���� �ڵ� */    return Input.GetKeyDown(KeyCode.Space); }
    public bool PlayerDidAttack() { /* ���� ���� �ڵ� */   return Input.GetMouseButtonDown(0); }
    public bool PlayerDidSave() { /* ���� ���� �ڵ� */   return Input.GetKeyDown(KeyCode.None); }
    public bool PlayerDidRun() { /* ���� ���� �ڵ� */   return Input.GetKey(KeyCode.None); }
    public bool PlayerDidProduction() { /* ���� ���� �ڵ� */   return Input.GetKey(KeyCode.None); }

    // UI Show/Hide �޼��� ����
    public void ShowMoveGuideUI(bool show) => moveGuideUI?.SetActive(show);
    public void ShowJumpGuideUI(bool show) => jumpGuideUI?.SetActive(show);
    public void ShowAttackGuideUI(bool show) => attackGuideUI?.SetActive(show);
    public void ShowSaveGuideUI(bool show) => saveGuideUI?.SetActive(show);
    public void ShowRunGuideUI(bool show) => runGuideUI?.SetActive(show);
    public void ShowProductionGuideUI(bool show) => productionGuidUI?.SetActive(show);
}