using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasGroupSelf;

    [HideInInspector] public bool isOpenStandBy;

    public float waitTime;
    public float fadeInTime;
    public float fadeOutTime;

    Coroutine currentPopMessageRoutine;

    public void PopMessage_FadeInOut(string messageText = "")
    {
        if (currentPopMessageRoutine != null)
        {
            StopCoroutine(currentPopMessageRoutine);
        }

        // 메시지 커스텀 설정값이 있다면 텍스트 세팅
        if (messageText != "")
        {
            canvasGroupSelf.GetComponentInChildren<TMP_Text>().text = messageText;
        }

        gameObject.SetActive(true);
        currentPopMessageRoutine = StartCoroutine(PopMessageRoutine(canvasGroupSelf));
    }

    private IEnumerator PopMessageRoutine(CanvasGroup canvasGroup)
    {
        isOpenStandBy = false;

        canvasGroup.alpha = 0f;
        float time = 0f;

        while (time < fadeInTime)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeInTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(waitTime);
        time = 0;

        // 페이드 인 하는 시점에서 스탠바이 온
        isOpenStandBy = true;

        while (time < fadeOutTime)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
