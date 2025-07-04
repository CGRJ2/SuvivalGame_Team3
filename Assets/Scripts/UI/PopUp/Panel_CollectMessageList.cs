using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_CollectMessageList : MonoBehaviour
{
    public List<CollectMessageSlot> collectMessageSlots;
    public float popFadeTime;
    Coroutine currentPopMessageRoutine;

    public void PopMessage(string messageText)
    {
        if (currentPopMessageRoutine != null)
        {
            StopCoroutine(currentPopMessageRoutine);
        }

        collectMessageSlots[0].tmp_CollectMessage.text = messageText;
        CanvasGroup nowCanvasGroup = collectMessageSlots[0].GetComponent<CanvasGroup>();
        currentPopMessageRoutine = StartCoroutine(PopMessageRoutine(nowCanvasGroup, popFadeTime));
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
