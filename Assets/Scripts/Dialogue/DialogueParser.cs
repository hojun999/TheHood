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

        string[] data = csvData.text.Split(new char[]{'\n'}); // 행 분리


        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});     // 열 분리, row[0] : 1열 i행~, row[1] : 2열 i행~, row[2] : 3열 i행~ 

            //Debug.Log(row[0]);
            //Debug.Log(row[1]);
            //Debug.Log(row[2]);


            if (row[0].ToString() != "") // 공백칸 제외 npcName 값 저장
            {
                npcNames.Add(row[0]);
                Debug.Log("npcNAmes 추가");

                for (int j = 0; j < npcNames.Count; j++)
                {
                    Debug.Log(npcNames[j]);
                }

                if (row[1].ToString() != "") // 공백칸 제외 dialogueID 값 저장
                {
                    dialogueIDs.Add(int.Parse(row[1]));
                }
            }




            do  // csv파일에서 같은 dialogueID에 context 값들 대응 가능한 상태로 저장
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
