using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public DialogueLoader dialogueLoader;

    public void LoadMovingTutorialDialogue()
    {
        dialogueLoader.LoadCSV("MovingTutorial");
    }
    public void LoadInteractiveTutorialDialogue()
    {
        dialogueLoader.LoadCSV("InteractiveTutorial");
    }
    public void LoadBattleTutorialDialogue()
    {
        dialogueLoader.LoadCSV("BattleTutorial");
    }
    public void LoadBaseCampTutorialDialogue()
    {
        dialogueLoader.LoadCSV("BaseCampTutorial");
    }
}