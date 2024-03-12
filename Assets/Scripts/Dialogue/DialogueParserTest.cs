using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParserTest : MonoBehaviour
{
    public string _csvFileName;
    public NPCData[] _NPCdatas;
 
    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], 외부/내부 딕셔너리 구분 잘
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
        string[] data = csvData.text.Split('\n'); // 행 분리

        string currentNPCName = "";
        int currentDialogueID = 0;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(','); // 열 분리
            if (row[0] != "")   // npcName 처리
            {
                currentNPCName = row[0];
                //Debug.Log(row[0]);
                if (!dialogueBundleByNPCName.ContainsKey(currentNPCName))
                {
                    dialogueBundleByNPCName[currentNPCName] = new Dictionary<int, List<string>>();  // dictionary key값 초기화
                }
            }

            if (row[1] != "")   // dialogueID 처리
            {
                currentDialogueID = int.Parse(row[1]);
                //Debug.Log(row[1]);
                if (!dialogueBundleByNPCName[currentNPCName].ContainsKey(currentDialogueID))
                {
                    dialogueBundleByNPCName[currentNPCName][currentDialogueID] = new List<string>();
                }
            }
            //Debug.Log(row[2]);

            dialogueBundleByNPCName[currentNPCName][currentDialogueID].Add(row[2]); // 대사 추가
        }
    }

    void PrintDialogueBundle(Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName)
    {
        Debug.Log("추가된 대화 데이터 확인");

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
                Debug.Log("npcName에 맞는 데이터 뿌리기");
                Debug.Log(npcName);

                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName에 맞는 데이터 없음");
            }
        }
    }
}
