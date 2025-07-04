using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    // 플레이어에서 호출
    public void Interact();


    // Interactable 오브젝트 본인의 콜라이더에 플레이어를 인식 시 실행.
    public void ShowInteractableUI();

    // 플레이어의 상호작용 영역안에 들어오면 UI 팝업창 띄우기 (ex 줍기:E, 설치하기:E)
    // 플레이어CC의 List<IInteractable> interactablesInRange에 해당 오브젝트 넣어주기

    public void CloseInteractableUI();

}
