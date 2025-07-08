using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject DialoguePanel;
    public DialogueLoader loader;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Image portrait;

    private int currentIndex = 0;

    private void Update()
    {
       
    }
    public void ShowNextLine()
    {
        if (loader.lines == null || loader.lines.Count == 0)
        {
            Debug.Log("대사 데이터가 없습니다.");
            return;
        }

        if (currentIndex < loader.lines.Count)
        {
            var line = loader.lines[currentIndex];
            Debug.Log($"{line.speaker}: {line.dialogue}");
            DialoguePanel.SetActive(true);
            speakerText.text = line.speaker;
            dialogueText.text = line.dialogue;
            currentIndex++;
        }
        else
        {
            DialoguePanel.SetActive(false);
            Debug.Log("대화 끝!");
        }
    }
    public void LoadDialogue(string fileName)
    {
        loader.LoadCSV(fileName);
        ResetDialogue();
    }
    public void ResetDialogue()
    {
        currentIndex = 0;
    }
}
