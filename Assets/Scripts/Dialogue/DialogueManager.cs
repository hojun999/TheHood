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
                //Debug.Log(npcName);

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
        dialogueID = GetCurDialogueID(_NPCData);
        Debug.Log(dialogueID);

        SetDialogueData(_NPCData, dialogueID);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);
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
                IDValue = GetCurDialougeIDByQuestState(_NPCData);
                break;
            case NPCData.m_type.trador:
                IDValue = GetCurDialogueIDByItemCount();
                break;
        }

        return IDValue;
    }

    private int GetCurDialougeIDByQuestState(NPCData _NPCData)
    {
        QuestData_New _questData = _questManager.GetCurQuestData();
        QuestData_New.q_state questState = _questData.questState;

        int IDValue = 0;

        switch (questState)
        {
            case QuestData_New.q_state.before:
                IDValue = _NPCData.curID;
                break;
            case QuestData_New.q_state.progress:
                IDValue = _NPCData.curID + 10 * dialogueID;
                break;
            case QuestData_New.q_state.clear:
                IDValue = _NPCData.curID - 10 * dialogueID + 1;
                break;
        }

        return IDValue;
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

        switch (npcType)
        {
            case NPCData.m_type.client:
                if (dialogueIndex == data.m_dialogueDic[ID].Count)
                {
                    context = null;
                    EndTalk();
                }
                else
                {
                    context = data.m_dialogueDic[ID][dialogueIndex];
                }
                break;
            case NPCData.m_type.trador:
                if (true)   // true �κп� �κ�manager�� ���� �� ������ ���� ������ ���� bool �� ����
                {
                    if (dialogueIndex == data.m_dialogueDic[ID].Count)
                    {
                        //TradeEvent();
                        context = null;
                        EndTalk();

                    }
                    else
                    {
                        context = data.m_dialogueDic[ID][dialogueIndex];

                    }
                }
                break;
        }


        return context;

    }

    private void EndTalk()
    {
        Debug.Log("��ȭ ����");

        //dialogueUI.SetActive(false);
        _playerInteract.isPlayerInteracting = false;
        dialogueIndex = -1;     // ������ ��ȭ���� interact�� �Ǳ� ������ dialogueindex++;�� ����ʿ� ���� �ʱ�ȭ
        //_questManager.CheckQuestClear();
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
