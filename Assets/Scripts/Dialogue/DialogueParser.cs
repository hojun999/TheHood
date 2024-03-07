using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public DialogueData[] Parse(string _CSVFileName)
    {
        List<DialogueData> dialogueList = new List<DialogueData>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[]{'\n'}); // 엔터 기준 분리

        for (int i = 1; i < data.Length;)
        {
            //Debug.Log(data[i]);
            string[] row = data[i].Split(new char[] {','});     // 열 분리

            DialogueData dialogueData = new DialogueData();        // 대사 리스트 생성

            dialogueData.talkID = int.Parse(row[0]);        // string > int 변경
            Debug.Log(row[0]);

            dialogueData.npcName = row[1];
            Debug.Log(row[1]);

            List<string> contextList = new List<string>();

            do
            {
                contextList.Add(row[2]);
                Debug.Log(row[2]);

                if (++i < data.Length)
                    row = data[i].Split(',');
                else
                    break;

            }
            while (row[0].ToString() == "");

            dialogueData.dialogueContexts = contextList.ToArray();

            dialogueList.Add(dialogueData);
        }


        return dialogueList.ToArray();
    }

    private void Start()
    {
        Parse("DialogueData");
    }
}
