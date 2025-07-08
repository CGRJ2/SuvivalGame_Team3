using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    public List<DialogueLine> lines = new List<DialogueLine>();

    public void LoadCSV(string filename)
    {
        lines.Clear();
        TextAsset csvFile= Resources.Load<TextAsset>("Dialogue/"+filename);

        if (csvFile == null)
        {
            Debug.Log("CSV파일을 찾을 수 없습니다. : " + filename);
            return;
        }

        var reader = new StringReader(csvFile.text);
        bool firstLine = true;

        while (reader.Peek() > -1)
        {
            var line = reader.ReadLine();
            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            var values = line.Split('/');

            if (values.Length < 4)
            {
                continue;
            }

            DialogueLine dialogueLine = new DialogueLine();
            dialogueLine.id = int.Parse(values[0]);
            dialogueLine.speaker = values[1];
            dialogueLine.dialogue = values[2];
            lines.Add(dialogueLine);
        }
    }
}
