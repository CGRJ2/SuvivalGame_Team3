using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    // 상태가 시작될때.
    public abstract void Enter();
    
    // 해당 상태에서 동작을 담당 (상태전환, 스프라이트 전환 등등)
    public abstract void Update();
    
    // 물리 처리를 담당. 사용하지 않는 상태들도 있기때문에 가상함수로 선언.
    public virtual void FixedUpdate() { }
    
    // 상태가 끝날때.
    public abstract void Exit();
}


