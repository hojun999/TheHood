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

    [HideInInspector] public int dialogueIDValue;       // ��ȭ ID �� ���� ����
    private int dialogueIndex;                          // �� ��� ���� ���� ���� ����

    [HideInInspector] public bool canStartDialogue = true;
    #endregion                   // ����
    private void Awake()
    {
        _dialougeParser = gameObject.GetComponent<DialogueParser>();                
        i_dialogueBundleByNPCName = _dialougeParser.Parse(_CSVFileName_dialogue);
    }   // ��ȭ ������ �Ľ�

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
    // �ܼ�â���� ��ȭ ������ Ȯ��
    void PrintDialogueBundle(Dictionary<string, Dictionary<int, List<string>>> dialogueBundleByNPCName) 
    {
        Debug.Log("�߰��� ��ȭ ������ Ȯ��");

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
                Debug.Log("npcName�� �´� ������ ����");
            }
        }
    }

    public void StartDialogueOnInteract(GameObject scanObject)
    {
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObject.GetComponent<NPCData>();

        if (canStartDialogue)           // ��ȭ ���۶��� ��� ID �Ҵ�
        {
            SetCurDialogueIDByNPCType(_NPCData);
            CheckSubmitQuest2Items();   // ����Ʈ 2 ���� ������ ����

            canStartDialogue = false;
        }

        SetDialogueContext(_NPCData, dialogueIDValue);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);

    }   // ��ȭ ����

    private void SetDialogueContext(NPCData _NPCData, int ID)               // ��� ���
    {
        string dialogueContext = GetCurDialogueContext(_NPCData, ID);       // ��� ���
        i_dialogueContext_tmp.text = dialogueContext;                       // ��� UI �ֽ�ȭ

        MoveToNextDialogueContext();    // ��� �� ���� ������ ���[]index++
    }

    private string GetCurDialogueContext(NPCData _NPCData, int ID)      // ��簡 ���� ���� ��� ���� �ٷ� �ѱ��
    {
        string context;

        
        if (dialogueIndex == _NPCData.m_dialogueDic[ID].Count && _NPCData.NPCType == NPCData.m_type.client)
        {
            context = null;
            EndTalkOnClient();
        }// �Ƿ����� ��ȭ�� ������ ��, ����Ʈ�� ���� ���� Ȯ��
        else if(dialogueIndex == _NPCData.m_dialogueDic[ID].Count && _NPCData.NPCType == NPCData.m_type.trader)
        {
            context = null;
            EndTalkOnTrader();
        }// �ŷ� ������ ��ȭ index �ʱ�ȭ
        else
            context = _NPCData.m_dialogueDic[ID][dialogueIndex];

        return context;

    }

    private void MoveToNextDialogueContext()        // ���� ��� �ѱ��
    {
        dialogueIndex++;
    }

    private void SetCurDialogueIDByNPCType(NPCData _NPCData)       // npcŸ�Կ� ���� �ٸ� ��ȭ ID ȣ�� �и�
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

    private void SetDatasDialougeIDByQuestState(NPCData _NPCData)    // ����Ʈ ���� ���¿� ���� ��ȭ ID ȣ�� ��Ģ
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

    // ȹ���� Ư�� ������ ������ ���� ��ȭ ID ȣ��
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


    private void EndTalkOnClient()      // �� ��ȭ�� ������ ���� ó��
    {
        QuestData data = _questManager.GetCurQuestData();

        if (data.questState == QuestData.q_state.before)
        {
            _questManager.StartQuest(data);
            _questManager.questionmark.SetActive(true);
            _questManager.exclamationmark.SetActive(false);
        }

         _questManager.ExcuteOnQuestClear();

        canStartDialogue = true;        // �ٽ� ��ȭ �������� curID�� �ҷ����� ����
        dialogueIndex = -1;             // ������ ��ȭ���� interact�� �Ǳ� ������ dialogueindex++;�� ����ʿ� ���� �ʱ�ȭ


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

        quest2Condition.getItemCount++;     // �ߺ� ȣ�� ����
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

    public void SetNextDatasIDOnClient()        // �������� ���� ����Ʈ ���� �ѱ��
    {
        //_NPCData.SetNextID();
        _NPCdatas[1].SetNextID();
    }

}
