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

        string[] data = csvData.text.Split(new char[]{'\n'}); // 행 분리


        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});     // 열 분리, row[0](npcName) : 1열 i행, row[1](dialogueID) : 2열 i행, row[2](context) : 3열 i행 

            //Debug.Log(row[0]);
            //Debug.Log(row[1]);
            //Debug.Log(row[2]);


            if (row[0].ToString() != "") // 공백칸 제외 npcName 값 저장
            {

                for (int j = 0; j < npcNames.Count; j++)
                {
                    Debug.Log(npcNames[j]);
                }

                if (row[1].ToString() != "") // 공백칸 제외 dialogueID 값 저장
                {
                    dialogueIDs.Add(int.Parse(row[1]));
                }
            }

            do  // npcName에 따른 dialogueDic 저장
            {
                if (row[0].ToString() != "")                // 공백 제외
                {
                    Debug.Log("npcNAmes 추가");   
                    npcNames.Add(row[0]);                   // NPC 이름 리스트에 추가
                }

                if (row[1].ToString() != "")                // 공백 제외
                {
                    Debug.Log("dialogueID 추가");
                    dialogueIDs.Add(int.Parse(row[1]));     // 대사 ID 리스트에 추가
                }

                do  // dialogueID에 따른 대사 저장
                {
                    contextList.Add(row[2]);            // 대사 리스트에 할당

                    if (++i < data.Length)
                        row = data[i].Split(',');       //  데이터 줄 ','로 구분(각 저장된 데이터에 대응시키기 위함)
                    else
                    {
                        contextsArray = contextList.ToArray();       // 대사 배열로 변경
                        contextsArrayList.Add(contextsArray);     // 대사 추가
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
            for (int j = 0; j < dialogueIDs.Count; j++)        // ID에 따른 대화리스트 큐 구성, dialogueID 개수 = contextslist 개수
            {
                dialogueContextDicByID.Add(dialogueIDs[j], contextsArrayList[j]);

                dialogueContextQueueByID.Enqueue(dialogueContextDicByID);       // dialogueContextDicByID.Add(dialogueIDs[j], contextsArrayList[j]);에서 생성된 dictionary 값을 순서대로 저장
            }

            dialogueBundleByNPCName.Add(npcNames[i], dialogueContextQueueByID.Dequeue());       // npcname 개수 = 번들 개수, npcname에 맞는 대화 번들 구성
        }
    }
    void PrintDialogueBundle(Dictionary<string, Dictionary<int, string[]>> dialogueBundleByNPCName)
    {
        Debug.Log("추가된 대화 데이터 확인");

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
