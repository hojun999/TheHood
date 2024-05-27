using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public string csvFileName;
    public NPCData[] _NPCdatas;

    public Dictionary<string, Dictionary<int, List<string>>> Parse(string _csvFileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(_csvFileName);
        string[] data = csvData.text.Split('\n'); // 행으로 분리

        Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();

        string currentNPCName = "";
        int currentDialogueID = 0;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');      // 열 분리

            if (!string.IsNullOrEmpty(row[0]))      // NPC 이름 처리
            {
                currentNPCName = row[0].Trim();     // 공백 제거
                if (!dialogueBundleByNPCName.ContainsKey(currentNPCName))
                {
                    dialogueBundleByNPCName[currentNPCName] = new Dictionary<int, List<string>>();
                }
            }

            if (!string.IsNullOrEmpty(row[1]))      // Dialogue ID 처리
            {
                currentDialogueID = int.Parse(row[1].Trim());

                if (!dialogueBundleByNPCName[currentNPCName].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPCName][currentDialogueID] = new List<string>();
                }
            }

            string dialogueText = row[2].Trim().Replace("'", ",");
            dialogueBundleByNPCName[currentNPCName][currentDialogueID].Add(dialogueText); // 대사 추가
        }

        return dialogueBundleByNPCName;             // dictionary 반환
    }
}
