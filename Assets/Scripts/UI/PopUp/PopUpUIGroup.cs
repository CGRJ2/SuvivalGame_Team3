using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpUIGroup : MonoBehaviour
{
    public Panel_CollectMessageList CollectMessageUI;
    public Panel_InteractableUI interactableUI;


    private IEnumerator ShowAndFadeRoutine(GameObject panel, TMP_Text text, string msg, float duration)
    {
        text.text = msg;
        panel.SetActive(true);
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }
}
