using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpUIGroup : MonoBehaviour
{
    public Panel_CollectMessageList CollectMessageUI;
    public Panel_InteractableUI interactableUI;
    public Panel_RoomInfo RoomInfoUI;

    public CanvasGroup message_Saved;


    [Header("팝업 메시지 설정")]
    public float popFadeTime;
    Coroutine currentPopMessageRoutine;

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
