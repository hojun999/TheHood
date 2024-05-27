using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region
    private PlayerInteract _playerInteract;
    private PlayerStat _playerStat;
    private DialogueParser _dialougeParser;
    private QuestManager _questManager;
    private InventoryManager _inventoryManager;
    private Quest2Condition quest2Condition;

    [SerializeField] private string _CSVFileName_dialogue;

    Dictionary<string, Dictionary<int, List<string>>> i_dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();
    public NPCData[] _NPCdatas;

    public GameObject dialogueUI;
    public TextMeshProUGUI i_dialogueContext_tmp;

    [HideInInspector] public int dialogueIDValue;       // 대화 ID 값 저장 변수
    private int dialogueIndex;                          // 각 대사 진행 순서 저장 변수

    [HideInInspector] public bool canStartDialogue = true;
    #endregion                   // 변수
    private void Awake()
    {
        _dialougeParser = gameObject.GetComponent<DialogueParser>();                
        i_dialogueBundleByNPCName = _dialougeParser.Parse(_CSVFileName_dialogue);
    }   // 대화 데이터 파싱

    private void Start()
    {
        SetOnStart();
    }
    void SetOnStart()
    {
        _playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
        _playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        _questManager = gameObject.GetComponent<QuestManager>();
        _inventoryManager = gameObject.GetComponent<InventoryManager>();
        quest2Condition = (Quest2Condition)_questManager.questConditionDatas[1];

        PrintDialogueBundle(i_dialogueBundleByNPCName);
        SaveDataOnNPCObject(i_dialogueBundleByNPCName);
    }
    // 콘솔창에서 대화 데이터 확인
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
                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName에 맞는 데이터 없음");
            }
        }
    }

    public void StartDialogueOnInteract(GameObject scanObject)
    {
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObject.GetComponent<NPCData>();

        if (canStartDialogue)           // 대화 시작때만 대사 ID 할당
        {
            SetCurDialogueIDByNPCType(_NPCData);
            CheckSubmitQuest2Items();   // 퀘스트 2 습득 아이템 제출

            canStartDialogue = false;
        }

        SetDialogueContext(_NPCData, dialogueIDValue);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);

    }   // 대화 시작

    private void SetDialogueContext(NPCData _NPCData, int ID)               // 대사 출력
    {
        string dialogueContext = GetCurDialogueContext(_NPCData, ID);       // 대사 얻기
        i_dialogueContext_tmp.text = dialogueContext;                       // 대사 UI 최신화

        MoveToNextDialogueContext();    // 대사 한 줄이 끝나면 대사[]index++
    }

    private string GetCurDialogueContext(NPCData _NPCData, int ID)      // 대사가 여러 줄인 경우 다음 줄로 넘기기
    {
        string context;

        
        if (dialogueIndex == _NPCData.m_dialogueDic[ID].Count && _NPCData.NPCType == NPCData.m_type.client)
        {
            context = null;
            EndTalkOnClient();
        }// 의뢰인은 대화가 끝났을 때, 퀘스트의 상태 변경 확인
        else if(dialogueIndex == _NPCData.m_dialogueDic[ID].Count && _NPCData.NPCType == NPCData.m_type.trader)
        {
            context = null;
            EndTalkOnTrader();
        }// 거래 상인은 대화 index 초기화
        else
            context = _NPCData.m_dialogueDic[ID][dialogueIndex];

        return context;

    }

    private void MoveToNextDialogueContext()        // 다음 대사 넘기기
    {
        dialogueIndex++;
    }

    private void SetCurDialogueIDByNPCType(NPCData _NPCData)       // npc타입에 따라 다른 대화 ID 호출 분리
    {
        NPCData.m_type npcType = _NPCData.NPCType;

        switch (npcType)
        {
            case NPCData.m_type.client:
                SetDatasDialougeIDByQuestState(_NPCData);
                break;
            case NPCData.m_type.trader:
                if (_NPCData.gameObject.name == "WeaponTrader")
                {
                    if (_inventoryManager.CheckItem(1002, 3))
                    {
                        GetCurDialogueIDOnWeaponTrador(_NPCData, _inventoryManager.CheckItem(1002, 3));
                    }
                    else
                    {
                        dialogueIDValue = _NPCData.curID;
                    }
                }
                else if (_NPCData.gameObject.name == "PosionTrader")
                {
                    if (_inventoryManager.CheckItem(1000, 2) && _inventoryManager.CheckItem(1001, 2))
                    {
                        GetCurDialogueIDOnPosionTrador(_NPCData, _inventoryManager.CheckItem(1000, 2), _inventoryManager.itemDicByID[100]);
                        GetCurDialogueIDOnPosionTrador(_NPCData, _inventoryManager.CheckItem(1001, 2), _inventoryManager.itemDicByID[101]);
                    }
                    else if (_inventoryManager.CheckItem(1000, 2))
                    {
                        GetCurDialogueIDOnPosionTrador(_NPCData, _inventoryManager.CheckItem(1000, 2), _inventoryManager.itemDicByID[100]);
                    }
                    else if (_inventoryManager.CheckItem(1001, 2))
                    {
                        GetCurDialogueIDOnPosionTrador(_NPCData, _inventoryManager.CheckItem(1001, 2), _inventoryManager.itemDicByID[101]);
                    }
                    else
                    {
                        dialogueIDValue = _NPCData.curID;
                    }
                }
                break;
        }
    }

    private void SetDatasDialougeIDByQuestState(NPCData _NPCData)    // 퀘스트 진행 상태에 따른 대화 ID 호출 규칙
    {
        int npcDataID = _NPCData.curID;
        QuestData _questData = _questManager.GetCurQuestData();
        QuestData.q_state questState = _questData.questState;

        switch (questState)
        {
            case QuestData.q_state.before:              
                dialogueIDValue = npcDataID;            
                break;
            case QuestData.q_state.progress:            
                dialogueIDValue = npcDataID * 10 + 1;
                break;
        }
    }

    // 획득한 특정 아이템 개수에 따른 대화 ID 호출
    void GetCurDialogueIDOnWeaponTrador(NPCData _NPCData, bool isAchieveItemCondition)
    {
        int npcDataID = _NPCData.curID;

        if (!isAchieveItemCondition)
        {
            dialogueIDValue = npcDataID;
        }
        else
        {
            dialogueIDValue = npcDataID + 1;
            _playerStat.maxHp += 20;
            _inventoryManager.TradeItem();
        }
    }

    void GetCurDialogueIDOnPosionTrador(NPCData _NPCData, bool isAchieveItemCondition, ItemData beAddedItem)  
    {
        int npcDataID = _NPCData.curID;

        if (!isAchieveItemCondition)
        {
            dialogueIDValue = npcDataID;
        }
        else
        {
            dialogueIDValue = npcDataID + 1;
            _inventoryManager.AddItemInInventory(beAddedItem);
            _inventoryManager.TradeItem();
        }
    }


    private void EndTalkOnClient()      // 각 대화가 끝났을 때의 처리
    {
        QuestData data = _questManager.GetCurQuestData();

        if (data.questState == QuestData.q_state.before)
        {
            _questManager.StartQuest(data);
            _questManager.questionmark.SetActive(true);
            _questManager.exclamationmark.SetActive(false);
        }

         _questManager.ExcuteOnQuestClear();

        canStartDialogue = true;        // 다시 대화 데이터의 curID를 불러오기 위함
        dialogueIndex = -1;             // 마지막 대화때도 interact가 되기 때문에 dialogueindex++;이 실행됨에 따른 초기화


        _playerInteract.isPlayerInteracting = false;
    }

    void SubmitQuest2Items()
    {
        for (int i = 0; i < _inventoryManager.inventorySlots.Length; i++)
        {
            if(_inventoryManager.inventorySlots[i].transform.childCount != 0)
            {
                Inventory_Item itemInSlot = _inventoryManager.inventorySlots[i].transform.GetChild(0).GetComponent<Inventory_Item>();

                if (itemInSlot.item.itemID == 1 ||
                    itemInSlot.item.itemID == 2 ||
                    itemInSlot.item.itemID == 3)
                {
                    itemInSlot.count--;
                    itemInSlot.RefreshCount();
                }
            }
        }

        quest2Condition.getItemCount++;     // 중복 호출 방지
    }

    void CheckSubmitQuest2Items()
    {
        if (_questManager.questIndex == 2 && quest2Condition.getItemCount == 3)
            SubmitQuest2Items();
    }

    private void EndTalkOnTrader()
    {
        canStartDialogue = true;
        dialogueIndex = -1;

        _playerInteract.isPlayerInteracting = false;
    }

    public void SetNextDatasIDOnClient()        // 데이터의 다음 퀘스트 대사로 넘기기
    {
        //_NPCData.SetNextID();
        _NPCdatas[1].SetNextID();
    }

}
