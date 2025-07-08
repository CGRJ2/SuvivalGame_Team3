using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public string csvFileName;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(hasTriggered)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            dialogueManager.LoadDialogue(csvFileName);
            dialogueManager.ShowNextLine();
        }
    }
}
