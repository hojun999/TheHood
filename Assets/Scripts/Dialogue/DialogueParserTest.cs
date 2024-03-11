using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParserTest : MonoBehaviour
{
    public DialogueManager dialogueManager; // 이 부분은 실제 DialogueManager 클래스가 필요합니다.

    public string csvFileName;

    private Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();

    private void Start()
    {
        Parse(csvFileName);
        PrintDialogueBundle(dialogueBundleByNPCName);
        Debug.Log("start");
    }

    public void Parse(string _CSVFileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split('\n'); // 행 분리

        string currentNPC = "";
        int currentDialogueID = 0;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(','); // 열 분리
            if (row[0] != "") // 새 NPC
            {
                currentNPC = row[0];
                //Debug.Log(row[0]);
                if (!dialogueBundleByNPCName.ContainsKey(currentNPC))
                {
                    dialogueBundleByNPCName[currentNPC] = new Dictionary<int, List<string>>();
                }
            }

            if (row[1] != "") // 새 Dialogue ID
            {
                currentDialogueID = int.Parse(row[1]);
                //Debug.Log(row[1]);
                if (!dialogueBundleByNPCName[currentNPC].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPC][currentDialogueID] = new List<string>();
                }
            }

            //Debug.Log(row[2]);

            dialogueBundleByNPCName[currentNPC][currentDialogueID].Add(row[2]); // 대사 추가
        }
    }

    void PrintDialogueBundle(Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName)
    {
        Debug.Log("추가된 대화 데이터 확인");

        foreach (var npcEntry in dialogueBundleByNPCName)
        {
            Debug.Log($"NPC Name: {npcEntry.Key}");
            foreach (var dialogueEntry in npcEntry.Value)
            {
                Debug.Log($"NPC Name: {npcEntry.Key}, Dialogue ID: {dialogueEntry.Key}, Dialogue: {string.Join(" / ", dialogueEntry.Value)}");
            }
        }
    }
}
