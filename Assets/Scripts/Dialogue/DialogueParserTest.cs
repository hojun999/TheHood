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
        string[] data = csvData.text.Split('\n'); // �� �и�

        string currentNPCName = "";
        int currentDialogueID = 0;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(','); // �� �и�
            if (row[0] != "")   // npcName ó��
            {
                currentNPCName = row[0];
                //Debug.Log(row[0]);
                if (!dialogueBundleByNPCName.ContainsKey(currentNPCName))
                {
                    dialogueBundleByNPCName[currentNPCName] = new Dictionary<int, List<string>>();  // dictionary key�� �ʱ�ȭ
                }
            }

            if (row[1] != "")   // dialogueID ó��
            {
                currentDialogueID = int.Parse(row[1]);
                if (!dialogueBundleByNPCName[currentNPCName].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPCName][currentDialogueID] = new List<string>();
                }
            }

            dialogueBundleByNPCName[currentNPCName][currentDialogueID].Add(row[2]); // ��� �߰�

        }
        return dialogueBundleByNPCName;

    }
}
