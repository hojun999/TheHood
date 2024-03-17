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

    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], �ܺ�/���� ��ųʸ� ���� ��
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
        //    // ��ȭ ID�� Ű�� ����Ͽ� ��ȭ ���ؽ�Ʈ �迭�� dictionary�� �Ҵ��մϴ�.
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
                //Debug.Log("npcName�� �´� ������ �Ѹ���");

                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName�� �´� ������ ����");
            }
        }
    }


    public void StartDialogueOnInteract(GameObject scanObj)
    {
        Debug.Log("��ȭ ����");
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObj.GetComponent<NPCData>();
        //dialogueID = _NPCData.curID;    //  ��ȭ�ϴ� npc�� ��ȭ ID�� ���� ���� ���� ����

        if (canStartDialogue)       // ��ȭ ���۶��� �´� ��� ID �Ҵ�
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
        Debug.Log("��ȭ ������ ����");

        string dialogueContext = GetCurDialogueContext(_NPCData, ID);
        i_dialogueContext_tmp.text = dialogueContext;

        MoveToNextDialogueContext();    // ��� �� ���� ������ ���[]index++
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

    private int GetCurDialougeIDExceptionByQuestState(NPCData _NPCData)    // ����Ʈ ���� ���¿� ���� ��ȭ ȣ�� ��Ģ
    {
        QuestData_New _questData = _questManager.GetCurQuestData();
        QuestData_New.q_state questState = _questData.questState;

        int curID = 0;
        int npcDataID = _NPCData.curID;

        switch (questState)             // ��ȭ �������� id�� ++�ϴ� ������ switch�� �ȿ��� ���� �޼ҵ�� ���� ó���ϴ� ������ ����!!
        {
            case QuestData_New.q_state.before:
                Debug.Log("�ش� npc�� ����Ʈ ���� ���´� ���� ��");
                curID = npcDataID;
                break;
            case QuestData_New.q_state.progress:
                curID = npcDataID + 10 * (dialogueID + 1);
                Debug.Log(curID);

                break;
            case QuestData_New.q_state.clear:
                Debug.Log("�ش� npc�� ����Ʈ ���� ���´� Ŭ����");
                curID = npcDataID - 10 * (dialogueID + 1) + 1;
                break;
        }

        Debug.Log("�ش� npc�� ID value ���� : " + curID);
        return curID;
    }

    private int GetCurDialogueIDByItemCount()
    {
        int IDValue = 0;

        return IDValue;
    }

    private string GetCurDialogueContext(NPCData data, int ID)
    {
        Debug.Log("��ȭ �� ���� ��� �ҷ�����");

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
        Debug.Log("��ȭ ����");

        QuestData_New data = _questManager.GetCurQuestData();

        if (data.questState == QuestData_New.q_state.before)    // ����Ʈ ���� ���̸� �ڵ������� ����Ʈ ����
            _questManager.StartQuest(data);


        _questManager.CheckQuestState();
        Debug.Log(data.questState);
        _playerInteract.isPlayerInteracting = false;
        canStartDialogue = true;
        dialogueIndex = -1;     // ������ ��ȭ���� interact�� �Ǳ� ������ dialogueindex++;�� ����ʿ� ���� �ʱ�ȭ

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
