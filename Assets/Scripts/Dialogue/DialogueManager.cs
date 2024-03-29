using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public PlayerInteract _playerInteract;
    private DialogueParser _dialougeParserTest;
    private QuestManager _questManager;

    [SerializeField] private string _CSVFileName_dialogue;

    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], 외부/내부 딕셔너리 구분 잘
    Dictionary<string, Dictionary<int, List<string>>> i_dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();
    public NPCData[] _NPCdatas;

    public GameObject dialogueUI;
    public TextMeshProUGUI i_dialogueContext_tmp;

    [HideInInspector] public int dialogueIDValue;       // 대화 ID 값 저장 변수
    private int dialogueIndex;                          // 각 대사 진행 순서 저장 변수

    [HideInInspector] public bool canStartDialogue = true;

    private void Awake()
    {
        _dialougeParserTest = gameObject.GetComponent<DialogueParser>();
        i_dialogueBundleByNPCName = _dialougeParserTest.Parse(_CSVFileName_dialogue);

        _questManager = gameObject.GetComponent<QuestManager>();
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
                //Debug.Log("각 npc에 대화 데이터 할당");

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
        //Debug.Log("대화 시작");
        //Debug.Log(canStartDialogue);
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObj.GetComponent<NPCData>();

        if (canStartDialogue)       // 대화 시작때만 맞는 대사 ID 할당
        {
            //int i_curID = CurDialogueIDByNPCType(_NPCData);
            SetCurDialogueIDByNPCType(_NPCData);

            canStartDialogue = false;
        }

        SetDialogueContext(_NPCData, dialogueIDValue);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);

    }

    private void SetDialogueContext(NPCData _NPCData, int ID)
    {

        //Debug.Log("대화 데이터 세팅");

        string dialogueContext = GetCurDialogueContext(_NPCData, ID);       // 대사 얻기
        i_dialogueContext_tmp.text = dialogueContext;                       // 대사 UI 최신화

        MoveToNextDialogueContext();    // 대사 한 줄이 끝나면 대사[]index++
    }

    private void SetCurDialogueIDByNPCType(NPCData _NPCData)       // npc타입에 따라 다른 대화 ID 호출 분리
    {
        NPCData.m_type npcType = _NPCData.NPCType;
        //int IDValue = 0;

        switch (npcType)
        {
            case NPCData.m_type.client:
                //IDValue = CurDatasDialougeIDByQuestState(_NPCData);
                SetDatasDialougeIDByQuestState(_NPCData);
                break;
            case NPCData.m_type.trador:
                //IDValue = GetCurDialogueIDByItemCount();
                GetCurDialogueIDByItemCount();
                break;
        }

        //Debug.Log("해당 npc의 ID value 값은 : " + IDValue);
        //return IDValue;
    }

    private void SetDatasDialougeIDByQuestState(NPCData _NPCData)    // 퀘스트 진행 상태에 따른 대화 ID 호출 규칙
    {
        //Debug.Log("현재 대화 데이터의 ID 값 호출");
        QuestData _questData = _questManager.GetCurQuestData();
        QuestData.q_state questState = _questData.questState;

        //int curIDvalue = 0;
        int npcDataID = _NPCData.curID;
        Debug.Log(npcDataID);
        switch (questState)
        {
            case QuestData.q_state.before:              // 해당 퀘스트가 진행 전이면
                dialogueIDValue = npcDataID;            // 대화 데이터의 ID 할당
                break;
            case QuestData.q_state.progress:            // 해당 퀘스트가 진행 중이면
                dialogueIDValue = npcDataID * 10 + 1;   // 진행 중일 때의 대화 데이터 ID 할당(규칙)
                break;
        }
        //Debug.Log(curIDvalue);
        //return curIDvalue;
    }

    private void GetCurDialogueIDByItemCount()  // 인벤토리에 보관하고 있는 특정 아이템 개수에 따른 대화 ID 호출
    {
        //int IDValue = 0;

        //return IDValue;
    }

    private string GetCurDialogueContext(NPCData _NPCData, int ID)      // 대사가 여러 줄인 경우 다음 줄로 넘기기
    {
        //Debug.Log("대화 각 줄의 대사 불러오기");

        string context;

        if(dialogueIndex == _NPCData.m_dialogueDic[ID].Count)
        {
            //Debug.Log("해당 대화 줄 끝");
            context = null;
            EndTalk();
        }
        else
            context = _NPCData.m_dialogueDic[ID][dialogueIndex];

        return context;

    }

    private void EndTalk()      // 각 대화가 끝났을 때의 처리
    {

        QuestData data = _questManager.GetCurQuestData();

        if (data.questState == QuestData.q_state.before)
        {
            _questManager.StartQuest(data);
            _questManager.questionmark.SetActive(false);
            _questManager.exclamationmark.SetActive(false);
        }

        canStartDialogue = true;                        // 다시 대화 데이터의 curID를 불러오기 위함
        dialogueIndex = -1;                             // 마지막 대화때도 interact가 되기 때문에 dialogueindex++;이 실행됨에 따른 초기화

        _playerInteract.isPlayerInteracting = false;

    }

    public void SetNextDatasID(NPCData _NPCData)        // 데이터의 다음 퀘스트 대사로 넘기기
    {
            _NPCData.SetNextID();
    }

    private void MoveToNextDialogueContext()        // 다음 대사 넘기기
    {
        dialogueIndex++;
    }
}
