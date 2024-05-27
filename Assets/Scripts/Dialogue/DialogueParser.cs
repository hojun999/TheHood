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
        string[] data = csvData.text.Split('\n'); // ������ �и�

        Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();

        string currentNPCName = "";
        int currentDialogueID = 0;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');      // �� �и�

            if (!string.IsNullOrEmpty(row[0]))      // NPC �̸� ó��
            {
                currentNPCName = row[0].Trim();     // ���� ����
                if (!dialogueBundleByNPCName.ContainsKey(currentNPCName))
                {
                    dialogueBundleByNPCName[currentNPCName] = new Dictionary<int, List<string>>();
                }
            }

            if (!string.IsNullOrEmpty(row[1]))      // Dialogue ID ó��
            {
                currentDialogueID = int.Parse(row[1].Trim());

                if (!dialogueBundleByNPCName[currentNPCName].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPCName][currentDialogueID] = new List<string>();
                }
            }

            string dialogueText = row[2].Trim().Replace("'", ",");
            dialogueBundleByNPCName[currentNPCName][currentDialogueID].Add(dialogueText); // ��� �߰�
        }

        return dialogueBundleByNPCName;             // dictionary ��ȯ
    }
}
