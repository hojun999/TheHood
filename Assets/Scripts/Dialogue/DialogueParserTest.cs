using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParserTest : MonoBehaviour
{
    public string _csvFileName;
    public NPCData[] _NPCdatas;
 
    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], �ܺ�/���� ��ųʸ� ���� ��
    private Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();

    private void Start()
    {
        Parse(_csvFileName);
        //PrintDialogueBundle(dialogueBundleByNPCName);
        SaveDataOnNPCObject();
        //PrintDialogueBundle(dialogueBundleByNPCName);
    }

    public void Parse(string _CSVFileName)
    {
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
                //Debug.Log(row[1]);
                if (!dialogueBundleByNPCName[currentNPCName].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPCName][currentDialogueID] = new List<string>();
                }
            }
            //Debug.Log(row[2]);

            dialogueBundleByNPCName[currentNPCName][currentDialogueID].Add(row[2]); // ��� �߰�
        }
    }

    void PrintDialogueBundle(Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName)
    {
        Debug.Log("�߰��� ��ȭ ������ Ȯ��");

        foreach (var npcNameEntry in dialogueBundleByNPCName)
        {
            foreach (var dialogueDic in npcNameEntry.Value)
            {
                Debug.Log($"NPC Name: {npcNameEntry.Key}, Dialogue ID: {dialogueDic.Key}, Dialogue: {string.Join(" / ", dialogueDic.Value)}");
            }
        }
    }

    void SaveDataOnNPCObject()
    {

        for (int i = 0; i < _NPCdatas.Length; i++)
        {
            string npcName = _NPCdatas[i].m_name;

            if (dialogueBundleByNPCName.ContainsKey(npcName))
            {
                Debug.Log("npcName�� �´� ������ �Ѹ���");
                Debug.Log(npcName);

                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName�� �´� ������ ����");
            }
        }
    }
}
