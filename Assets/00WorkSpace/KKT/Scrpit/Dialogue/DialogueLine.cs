using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLine", menuName = "Dialogue/DialogueLine", order = 0)]
public class DialogueLine : ScriptableObject
{
    public int id;
    public string speaker;
    [TextArea(2, 5)]
    public string dialogue;
    // public string position;
    // public Sprite portrait;
    // public AudioClip sfx;
}
