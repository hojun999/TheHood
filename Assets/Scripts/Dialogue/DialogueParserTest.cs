using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParserTest : MonoBehaviour
{
    public string _csvFileName;
    public NPCData[] _NPCdatas;

    public Dictionary<string, Dictionary<int, List<string>>> Parse(string _CSVFileName)
    {
        Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();

        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split('\n'); // 행 분리

        string currentNPCName = "";
        int currentDialogueID = 0;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(','); // 열 분리
            if (row[0] != "")   // npcName 처리
            {
                currentNPCName = row[0];
                //Debug.Log(row[0]);
                if (!dialogueBundleByNPCName.ContainsKey(currentNPCName))
                {
                    dialogueBundleByNPCName[currentNPCName] = new Dictionary<int, List<string>>();  // dictionary key값 초기화
                }
            }

            if (row[1] != "")   // dialogueID 처리
            {
                currentDialogueID = int.Parse(row[1]);
                if (!dialogueBundleByNPCName[currentNPCName].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPCName][currentDialogueID] = new List<string>();
                }
            }

            dialogueBundleByNPCName[currentNPCName][currentDialogueID].Add(row[2]); // 대사 추가

        }
        return dialogueBundleByNPCName;

    }
}
