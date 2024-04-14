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
    private InventoryManager _inventoryManager;

    [SerializeField] private string _CSVFileName_dialogue;

    Dictionary<string, Dictionary<int, List<string>>> i_dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();
    public NPCData[] _NPCdatas;

    public GameObject dialogueUI;
    public TextMeshProUGUI i_dialogueContext_tmp;

    [HideInInspector] public int dialogueIDValue;       // ��ȭ ID �� ���� ����
    private int dialogueIndex;                          // �� ��� ���� ���� ���� ����

    [HideInInspector] public bool canStartDialogue = true;

    private void Awake()
    {
        _dialougeParserTest = gameObject.GetComponent<DialogueParser>();
        i_dialogueBundleByNPCName = _dialougeParserTest.Parse(_CSVFileName_dialogue);
        _questManager = gameObject.GetComponent<QuestManager>();
        _inventoryManager = gameObject.GetComponent<InventoryManager>();
    }

    private void Start()
    {
        //PrintDialogueBundle(i_dialogueBundleByNPCName);
        SaveDataOnNPCObject(i_dialogueBundleByNPCName);
    }

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
                //Debug.Log("�� npc�� ��ȭ ������ �Ҵ�");

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
        //Debug.Log("��ȭ ����");
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObject.GetComponent<NPCData>();

        if (canStartDialogue)       // ��ȭ ���۶��� �´� ��� ID �Ҵ�
        {
            SetCurDialogueIDByNPCType(_NPCData);

            canStartDialogue = false;
        }

        SetDialogueContext(_NPCData, dialogueIDValue);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);

    }

    private void SetDialogueContext(NPCData _NPCData, int ID)
    {

        //Debug.Log("��ȭ ������ ����");

        string dialogueContext = GetCurDialogueContext(_NPCData, ID);       // ��� ���
        i_dialogueContext_tmp.text = dialogueContext;                       // ��� UI �ֽ�ȭ

        MoveToNextDialogueContext();    // ��� �� ���� ������ ���[]index++
    }
    private string GetCurDialogueContext(NPCData _NPCData, int ID)      // ��簡 ���� ���� ��� ���� �ٷ� �ѱ��
    {
        //Debug.Log("��ȭ �� ���� ��� �ҷ�����");

        string context;

        if (dialogueIndex == _NPCData.m_dialogueDic[ID].Count && _NPCData.NPCType == NPCData.m_type.client)     // �Ƿ����� ��ȭ�� ������ ��, ����Ʈ�� ���� ���� Ȯ��
        {
            //Debug.Log("�ش� ��ȭ �� ��");
            context = null;
            EndTalkOnClient();
        }
        else if(dialogueIndex == _NPCData.m_dialogueDic[ID].Count && _NPCData.NPCType == NPCData.m_type.trader)     // �ŷ� ������ ��ȭ index �ʱ�ȭ
        {
            context = null;
            EndTalkOnTrader();
        }
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
                    GetCurDialogueIDByItemCount(_NPCData, _inventoryManager.CheckItem(1002, 3), _inventoryManager.itemDicByID[1002]);           // ���� ü�� ������Ű�� �ɷ� ����
                else if (_NPCData.gameObject.name == "PosionTrader")
                    GetCurDialogueIDByItemCount(_NPCData, _inventoryManager.CheckItem(1000, 2), _inventoryManager.itemDicByID[100]);
                else if (_NPCData.gameObject.name == "PosionTrader")
                    GetCurDialogueIDByItemCount(_NPCData, _inventoryManager.CheckItem(1001, 2), _inventoryManager.itemDicByID[101]);
                break;
        }

    }

    private void SetDatasDialougeIDByQuestState(NPCData _NPCData)    // ����Ʈ ���� ���¿� ���� ��ȭ ID ȣ�� ��Ģ
    {
        //Debug.Log("���� ��ȭ �������� ID �� ȣ��");

        int npcDataID = _NPCData.curID;
        QuestData _questData = _questManager.GetCurQuestData();
        QuestData.q_state questState = _questData.questState;

        switch (questState)
        {
            case QuestData.q_state.before:              // �ش� ����Ʈ�� ���� ���̸�
                dialogueIDValue = npcDataID;            // ��ȭ �������� ID �Ҵ�
                break;
            case QuestData.q_state.progress:            // �ش� ����Ʈ�� ���� ���̸�
                dialogueIDValue = npcDataID * 10 + 1;   // ���� ���� ���� ��ȭ ������ ID �Ҵ�(��Ģ)
                break;
        }
    }

    private void GetCurDialogueIDByItemCount(NPCData _NPCData, bool isAchieveItemCondition, ItemData beAddedItem)  // �κ��丮�� �����ϰ� �ִ� Ư�� ������ ������ ���� ��ȭ ID ȣ��
    {
        int npcDataID = _NPCData.curID;

        if (!isAchieveItemCondition)
        {
            Debug.Log("��ȯ�� �ʿ��� ������ ����");
            dialogueIDValue = npcDataID;
        }
        else
        {
            Debug.Log("��ȯ ����");
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
            _questManager.questionmark.SetActive(false);
            _questManager.exclamationmark.SetActive(false);
        }

        canStartDialogue = true;                        // �ٽ� ��ȭ �������� curID�� �ҷ����� ����
        dialogueIndex = -1;                             // ������ ��ȭ���� interact�� �Ǳ� ������ dialogueindex++;�� ����ʿ� ���� �ʱ�ȭ

        _playerInteract.isPlayerInteracting = false;

    }

    private void EndTalkOnTrader()
    {
        canStartDialogue = true;
        dialogueIndex = -1;

        _playerInteract.isPlayerInteracting = false;
    }

    public void SetNextDatasID(NPCData _NPCData)        // �������� ���� ����Ʈ ���� �ѱ��
    {
            _NPCData.SetNextID();
    }

}
