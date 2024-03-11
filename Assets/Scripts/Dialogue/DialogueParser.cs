using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private List<string> npcNames = new List<string>();

    private List<int> dialogueIDs = new List<int>();

    private List<string> contextList = new List<string>();
    private string[] contextsArray;
    private List<string[]> contextsArrayList = new List<string[]>();

    Dictionary<int, string[]> dialogueContextDicByID = new Dictionary<int, string[]>();
    Queue<Dictionary<int, string[]>> dialogueContextQueueByID = new Queue<Dictionary<int, string[]>>();

    Dictionary<string, Dictionary<int, string[]>> dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, string[]>>();

    private void Start()
    {
        Parse("DialogueData");

        PrintDialogueBundle(dialogueBundleByNPCName);
    }


    public void Parse(string _CSVFileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[]{'\n'}); // �� �и�


        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});     // �� �и�, row[0](npcName) : 1�� i��, row[1](dialogueID) : 2�� i��, row[2](context) : 3�� i�� 

            //Debug.Log(row[0]);
            //Debug.Log(row[1]);
            //Debug.Log(row[2]);


            if (row[0].ToString() != "") // ����ĭ ���� npcName �� ����
            {

                for (int j = 0; j < npcNames.Count; j++)
                {
                    Debug.Log(npcNames[j]);
                }

                if (row[1].ToString() != "") // ����ĭ ���� dialogueID �� ����
                {
                    dialogueIDs.Add(int.Parse(row[1]));
                }
            }

            do  // npcName�� ���� dialogueDic ����
            {
                if (row[0].ToString() != "")                // ���� ����
                {
                    Debug.Log("npcNAmes �߰�");   
                    npcNames.Add(row[0]);                   // NPC �̸� ����Ʈ�� �߰�
                }

                if (row[1].ToString() != "")                // ���� ����
                {
                    Debug.Log("dialogueID �߰�");
                    dialogueIDs.Add(int.Parse(row[1]));     // ��� ID ����Ʈ�� �߰�
                }

                do  // dialogueID�� ���� ��� ����
                {
                    contextList.Add(row[2]);            // ��� ����Ʈ�� �Ҵ�

                    if (++i < data.Length)
                        row = data[i].Split(',');       //  ������ �� ','�� ����(�� ����� �����Ϳ� ������Ű�� ����)
                    else
                    {
                        contextsArray = contextList.ToArray();       // ��� �迭�� ����
                        contextsArrayList.Add(contextsArray);     // ��� �߰�
                        //dialogueDic.Add(int.Parse(row[1]), contextArray);

                        break;
                    }

                }
                while (row[1].ToString() == "");
            }
            while (row[0].ToString() == "");

            HandleDialogueDatas();
        }

        //for (int i = 0; i < npcNames.Count; i++)
        //{
        //    //dialogueManager.dialogueBundleDic.Add(npcNames[i], )
        //}
    }

    void HandleDialogueDatas()
    {
        for (int i = 0; i < npcNames.Count; i++)
        {
            for (int j = 0; j < dialogueIDs.Count; j++)        // ID�� ���� ��ȭ����Ʈ ť ����, dialogueID ���� = contextslist ����
            {
                dialogueContextDicByID.Add(dialogueIDs[j], contextsArrayList[j]);

                dialogueContextQueueByID.Enqueue(dialogueContextDicByID);       // dialogueContextDicByID.Add(dialogueIDs[j], contextsArrayList[j]);���� ������ dictionary ���� ������� ����
            }

            dialogueBundleByNPCName.Add(npcNames[i], dialogueContextQueueByID.Dequeue());       // npcname ���� = ���� ����, npcname�� �´� ��ȭ ���� ����
        }
    }
    void PrintDialogueBundle(Dictionary<string, Dictionary<int, string[]>> dialogueBundleByNPCName)
    {
        Debug.Log("�߰��� ��ȭ ������ Ȯ��");

        foreach (var npcName in dialogueBundleByNPCName.Keys)
        {
            Debug.Log($"NPC Name: {npcName}");
            foreach (var dialogue in dialogueBundleByNPCName[npcName])
            {
                Debug.Log($"Dialogue ID: {dialogue.Key}, Dialogue: {string.Join(" / ", dialogue.Value)}");
            }
        }
    }
}
