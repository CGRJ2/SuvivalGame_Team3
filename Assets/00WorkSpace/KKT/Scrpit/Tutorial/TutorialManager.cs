using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialStateMachine stateMachine;

    // 각종 튜토리얼 UI 오브젝트 참조
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
        stateMachine.ChangeState(TutorialStateType.Move); // 튜토리얼 시작
    }

    void Update()
    {
        stateMachine.Update();
    }

    // 플레이어 행동 감지 메서드 예시
    public bool PlayerDidMove() { /* 실제 이동 감지 코드 */ return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0; }
    public bool PlayerDidJump() { /* 점프 감지 코드 */    return Input.GetKeyDown(KeyCode.Space); }
    public bool PlayerDidAttack() { /* 공격 감지 코드 */   return Input.GetMouseButtonDown(0); }
    public bool PlayerDidSave() { /* 공격 감지 코드 */   return Input.GetKeyDown(KeyCode.None); }
    public bool PlayerDidRun() { /* 공격 감지 코드 */   return Input.GetKey(KeyCode.None); }
    public bool PlayerDidProduction() { /* 공격 감지 코드 */   return Input.GetKey(KeyCode.None); }

    // UI Show/Hide 메서드 예시
    public void ShowMoveGuideUI(bool show) => moveGuideUI?.SetActive(show);
    public void ShowJumpGuideUI(bool show) => jumpGuideUI?.SetActive(show);
    public void ShowAttackGuideUI(bool show) => attackGuideUI?.SetActive(show);
    public void ShowSaveGuideUI(bool show) => saveGuideUI?.SetActive(show);
    public void ShowRunGuideUI(bool show) => runGuideUI?.SetActive(show);
    public void ShowProductionGuideUI(bool show) => productionGuidUI?.SetActive(show);
}