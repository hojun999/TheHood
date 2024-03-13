using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public PlayerInteract _playerInteract;
    private DialogueParserTest _dialougeParserTest;

    [SerializeField] private string _CSVFileName_dialogue;

    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], 외부/내부 딕셔너리 구분 잘
    Dictionary<string, Dictionary<int, List<string>>> i_dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();
    public NPCData[] _NPCdatas;

    public GameObject dialogueUI;

    [HideInInspector] public int dialogueID;
    private int dialogueIndex;

    private void Awake()
    {
        _dialougeParserTest = gameObject.GetComponent<DialogueParserTest>();
        i_dialogueBundleByNPCName = _dialougeParserTest.Parse(_CSVFileName_dialogue);

        //DialogueData[] dialogueDatas = dialogueParser.Parse(_CSVFileName);

        //for (int i = 0; i < dialogueDatas.Length; i++)
        //{
        //    // 대화 ID를 키로 사용하여 대화 컨텍스트 배열을 dictionary에 할당합니다.
        //    dialogueDic.Add(dialogueDatas[i].talkID, dialogueDatas[i].dialogueContexts);
        //}

    }

    private void Start()
    {
        PrintDialogueBundle(i_dialogueBundleByNPCName);
        SaveDataOnNPCObject(i_dialogueBundleByNPCName);
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

    void SaveDataOnNPCObject(Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName)
    {

        for (int i = 0; i < _NPCdatas.Length; i++)
        {
            string npcName = _NPCdatas[i].m_name;

            if (dialogueBundleByNPCName.ContainsKey(npcName))
            {
                //Debug.Log("npcName에 맞는 데이터 뿌리기");
                Debug.Log(npcName);

                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName에 맞는 데이터 없음");
            }
        }
    }


    public void SetTalkData(GameObject scanObj)
    {
        NPCData _NPCData = scanObj.GetComponent<NPCData>();
        Talk(_NPCData, _NPCData.m_dialogueID, _NPCData.NPCType);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);
    }

    private void Talk(NPCData data,int ID, Enum type)
    {
        string dialogueContext = data.m_dialogueDic[ID][dialogueIndex];
    }


}
