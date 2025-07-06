using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpUIGroup : MonoBehaviour
{
    [Header("�˾� �޽��� �⺻ ����")]
    public float popFadeTime;


    [Header("�˾� �޽��� : ����")]
    public CanvasGroup message_Saved;
    [Header("�˾� �޽��� : ȹ��")]
    public Panel_CollectMessageList CollectMessageUI;
    
    [Header("�˾� �г� : ��ȣ�ۿ� ����")]
    public Panel_InteractableUI interactableUI;

    [Header("���̵� �� �� �ƿ� : ���� ��ġ")]
    public Panel_RoomInfo RoomInfoUI;
    [Header("���̵� �� �� �ƿ� : ���� �г�")]
    public Panel_FadeInOut deadPanel;
    [Header("���̵� �� �� �ƿ� : ���� �г�")]
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

        // �޽��� Ŀ���� �������� �ִٸ� �ؽ�Ʈ ����
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
