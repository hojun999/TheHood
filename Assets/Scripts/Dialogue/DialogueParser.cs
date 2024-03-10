using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public DialogueManager dialogueManager;

    List<string> npcNames = new List<string>();
    List<int> dialogueIDs = new List<int>();
    List<string> contextList = new List<string>();
    string[] contextArray;

    Dictionary<int, string[]> dialogueDic = new Dictionary<int, string[]>();

    private void Start()
    {
        Parse("DialogueData");

    }


    public void Parse(string _CSVFileName)
    {
        //List<DialogueData> dialogueList = new List<DialogueData>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[]{'\n'}); // �� �и�


        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});     // �� �и�, row[0] : 1�� i��~, row[1] : 2�� i��~, row[2] : 3�� i��~ 

            //Debug.Log(row[0]);
            //Debug.Log(row[1]);
            //Debug.Log(row[2]);


            if (row[0].ToString() != "") // ����ĭ ���� npcName �� ����
            {
                npcNames.Add(row[0]);
                Debug.Log("npcNAmes �߰�");

                for (int j = 0; j < npcNames.Count; j++)
                {
                    Debug.Log(npcNames[j]);
                }

                if (row[1].ToString() != "") // ����ĭ ���� dialogueID �� ����
                {
                    dialogueIDs.Add(int.Parse(row[1]));
                }
            }




            do  // csv���Ͽ��� ���� dialogueID�� context ���� ���� ������ ���·� ����
            {
                contextList.Add(row[2]);

                if (++i < data.Length)
                    row = data[i].Split(',');
                else
                {
                    contextArray = contextList.ToArray();
                    dialogueDic.Add(int.Parse(row[1]), contextArray);

                    break;
                }
                    
            }
            while (row[1].ToString() == "");


        }

        for (int i = 0; i < npcNames.Count; i++)
        {
            dialogueManager.dialogueBundleDic.Add(npcNames[i], )
        }

    }

}
