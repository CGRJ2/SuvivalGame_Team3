using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpUIGroup : MonoBehaviour
{
    [Header("팝업 메시지 기본 설정")]
    public float popFadeTime;


    [Header("팝업 메시지 : 저장")]
    public CanvasGroup message_Saved;
    [Header("팝업 메시지 : 획득")]
    public Panel_CollectMessageList CollectMessageUI;
    
    [Header("팝업 패널 : 상호작용 정보")]
    public Panel_InteractableUI interactableUI;

    [Header("페이드 인 앤 아웃 : 현재 위치")]
    public Panel_RoomInfo RoomInfoUI;
    [Header("페이드 인 앤 아웃 : 죽음 패널")]
    public Panel_FadeInOut deadPanel;
    [Header("페이드 인 앤 아웃 : 기절 패널")]
    public Panel_FadeInOut faintPanel;



    Coroutine currentPopMessageRoutine;
    private void Awake() => Init();


    private void Init()
    {
        UIManager.Instance.popUpUIGroup = this;
    }


    public void PopMessage(CanvasGroup canvasGroup, string messageText = "")
    {
        if (currentPopMessageRoutine != null)
        {
            StopCoroutine(currentPopMessageRoutine);
        }

        // 메시지 커스텀 설정값이 있다면 텍스트 세팅
        if (messageText != "")
        {
            canvasGroup.GetComponentInChildren<TMP_Text>().text = messageText;
        }

        currentPopMessageRoutine = StartCoroutine(PopMessageRoutine(canvasGroup, popFadeTime));
    }

    


    private IEnumerator PopMessageRoutine(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    

}
