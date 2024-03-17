using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public PlayerInteract _playerInteract;
    private DialogueParserTest _dialougeParserTest;
    private QuestManager_New _questManager;

    [SerializeField] private string _CSVFileName_dialogue;

    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], 외부/내부 딕셔너리 구분 잘
    Dictionary<string, Dictionary<int, List<string>>> i_dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();
    public NPCData[] _NPCdatas;

    public GameObject dialogueUI;
    public TextMeshProUGUI i_dialogueContext_tmp;

    [HideInInspector] public int dialogueID;
    private int dialogueIndex;

    private bool canStartDialogue = true;

    private void Awake()
    {
        _dialougeParserTest = gameObject.GetComponent<DialogueParserTest>();
        i_dialogueBundleByNPCName = _dialougeParserTest.Parse(_CSVFileName_dialogue);

        _questManager = gameObject.GetComponent<QuestManager_New>();

        //DialogueData[] dialogueDatas = dialogueParser.Parse(_CSVFileName);

        //for (int i = 0; i < dialogueDatas.Length; i++)
        //{
        //    // 대화 ID를 키로 사용하여 대화 컨텍스트 배열을 dictionary에 할당합니다.
        //    dialogueDic.Add(dialogueDatas[i].talkID, dialogueDatas[i].dialogueContexts);
        //}

    }

    private void Start()
    {
        //PrintDialogueBundle(i_dialogueBundleByNPCName);
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

                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName에 맞는 데이터 없음");
            }
        }
    }


    public void StartDialogueOnInteract(GameObject scanObj)
    {
        Debug.Log("대화 시작");
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObj.GetComponent<NPCData>();
        //dialogueID = _NPCData.curID;    //  대화하는 npc의 대화 ID에 따른 진행 순서 대응

        if (canStartDialogue)       // 대화 시작때만 맞는 대사 ID 할당
        {
            dialogueID = GetCurDialogueID(_NPCData);
            Debug.Log(dialogueID);
        }

        SetDialogueData(_NPCData, dialogueID);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);

        canStartDialogue = false;
    }

    private void SetDialogueData(NPCData _NPCData, int ID)
    {
        Debug.Log("대화 데이터 세팅");

        string dialogueContext = GetCurDialogueContext(_NPCData, ID);
        i_dialogueContext_tmp.text = dialogueContext;

        MoveToNextDialogueContext();    // 대사 한 줄이 끝나면 대사[]index++
    }

    private int GetCurDialogueID(NPCData _NPCData)
    {
        NPCData.m_type npcType = _NPCData.NPCType;
        int IDValue = 0;

        switch (npcType)
        {
            case NPCData.m_type.client:
                IDValue = GetCurDialougeIDExceptionByQuestState(_NPCData);
                break;
            case NPCData.m_type.trador:
                IDValue = GetCurDialogueIDByItemCount();
                break;
        }

        return IDValue;
    }

    private int GetCurDialougeIDExceptionByQuestState(NPCData _NPCData)    // 퀘스트 진행 상태에 따른 대화 호출 규칙
    {
        QuestData_New _questData = _questManager.GetCurQuestData();
        QuestData_New.q_state questState = _questData.questState;

        int curID = 0;
        int npcDataID = _NPCData.curID;

        switch (questState)             // 대화 데이터의 id를 ++하는 조건을 switch문 안에서 말고 메소드로 만들어서 처리하는 것으로 변경!!
        {
            case QuestData_New.q_state.before:
                Debug.Log("해당 npc의 퀘스트 진행 상태는 진행 전");
                curID = npcDataID;
                break;
            case QuestData_New.q_state.progress:
                curID = npcDataID + 10 * (dialogueID + 1);
                Debug.Log(curID);

                break;
            case QuestData_New.q_state.clear:
                Debug.Log("해당 npc의 퀘스트 진행 상태는 클리어");
                curID = npcDataID - 10 * (dialogueID + 1) + 1;
                break;
        }

        Debug.Log("해당 npc의 ID value 값은 : " + curID);
        return curID;
    }

    private int GetCurDialogueIDByItemCount()
    {
        int IDValue = 0;

        return IDValue;
    }

    private string GetCurDialogueContext(NPCData data, int ID)
    {
        Debug.Log("대화 각 줄의 대사 불러오기");

        NPCData.m_type npcType = data.NPCType;
        string context = "";

        if(dialogueIndex == data.m_dialogueDic[ID].Count)
        {
            context = null;
            EndTalk();
        }
        else
            context = data.m_dialogueDic[ID][dialogueIndex];

        return context;

    }

    private void EndTalk()
    {
        Debug.Log("대화 종료");

        QuestData_New data = _questManager.GetCurQuestData();

        if (data.questState == QuestData_New.q_state.before)    // 퀘스트 진행 전이면 자동적으로 퀘스트 시작
            _questManager.StartQuest(data);


        _questManager.CheckQuestState();
        Debug.Log(data.questState);
        _playerInteract.isPlayerInteracting = false;
        canStartDialogue = true;
        dialogueIndex = -1;     // 마지막 대화때도 interact가 되기 때문에 dialogueindex++;이 실행됨에 따른 초기화

    }

    public void MoveToNextDialogueID()
    {
        dialogueID++;
    }

    private void MoveToNextDialogueContext()
    {
        dialogueIndex++;
    }
}
