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
    private QuestManager _questManager;

    [SerializeField] private string _CSVFileName_dialogue;

    // dialogueBundleByNPCName[key][value(int : key, list<string> : value)], �ܺ�/���� ��ųʸ� ���� ��
    Dictionary<string, Dictionary<int, List<string>>> i_dialogueBundleByNPCName = new Dictionary<string, Dictionary<int, List<string>>>();
    public NPCData[] _NPCdatas;

    public GameObject dialogueUI;
    public TextMeshProUGUI i_dialogueContext_tmp;

    [HideInInspector] public int dialogueIDValue;       // ��ȭ ID �� ���� ����
    private int dialogueIndex;                          // �� ��� ���� ���� ���� ����

    [HideInInspector] public bool canStartDialogue = true;

    private void Awake()
    {
        _dialougeParserTest = gameObject.GetComponent<DialogueParserTest>();
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


    public void StartDialogueOnInteract(GameObject scanObj)
    {
        //Debug.Log("��ȭ ����");
        //Debug.Log(canStartDialogue);
        _playerInteract.isPlayerInteracting = true;

        NPCData _NPCData = scanObj.GetComponent<NPCData>();

        if (canStartDialogue)       // ��ȭ ���۶��� �´� ��� ID �Ҵ�
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

        //Debug.Log("��ȭ ������ ����");

        string dialogueContext = GetCurDialogueContext(_NPCData, ID);       // ��� ���
        i_dialogueContext_tmp.text = dialogueContext;                       // ��� UI �ֽ�ȭ

        MoveToNextDialogueContext();    // ��� �� ���� ������ ���[]index++
    }

    private void SetCurDialogueIDByNPCType(NPCData _NPCData)       // npcŸ�Կ� ���� �ٸ� ��ȭ ID ȣ�� �и�
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

        //Debug.Log("�ش� npc�� ID value ���� : " + IDValue);
        //return IDValue;
    }

    private void SetDatasDialougeIDByQuestState(NPCData _NPCData)    // ����Ʈ ���� ���¿� ���� ��ȭ ID ȣ�� ��Ģ
    {
        //Debug.Log("���� ��ȭ �������� ID �� ȣ��");
        QuestData _questData = _questManager.GetCurQuestData();
        QuestData.q_state questState = _questData.questState;

        //int curIDvalue = 0;
        int npcDataID = _NPCData.curID;
        Debug.Log(npcDataID);
        switch (questState)
        {
            case QuestData.q_state.before:              // �ش� ����Ʈ�� ���� ���̸�
                dialogueIDValue = npcDataID;            // ��ȭ �������� ID �Ҵ�
                break;
            case QuestData.q_state.progress:            // �ش� ����Ʈ�� ���� ���̸�
                dialogueIDValue = npcDataID * 10 + 1;   // ���� ���� ���� ��ȭ ������ ID �Ҵ�(��Ģ)
                break;
        }
        //Debug.Log(curIDvalue);
        //return curIDvalue;
    }

    private void GetCurDialogueIDByItemCount()  // �κ��丮�� �����ϰ� �ִ� Ư�� ������ ������ ���� ��ȭ ID ȣ��
    {
        //int IDValue = 0;

        //return IDValue;
    }

    private string GetCurDialogueContext(NPCData _NPCData, int ID)      // ��簡 ���� ���� ��� ���� �ٷ� �ѱ��
    {
        //Debug.Log("��ȭ �� ���� ��� �ҷ�����");

        string context;

        if(dialogueIndex == _NPCData.m_dialogueDic[ID].Count)
        {
            //Debug.Log("�ش� ��ȭ �� ��");
            context = null;
            EndTalk();
        }
        else
            context = _NPCData.m_dialogueDic[ID][dialogueIndex];

        return context;

    }

    private void EndTalk()      // �� ��ȭ�� ������ ���� ó��
    {

        QuestData data = _questManager.GetCurQuestData();

        if (data.questState == QuestData.q_state.before)
            _questManager.StartQuest(data);

        canStartDialogue = true;                        // �ٽ� ��ȭ �������� curID�� �ҷ����� ����
        dialogueIndex = -1;                             // ������ ��ȭ���� interact�� �Ǳ� ������ dialogueindex++;�� ����ʿ� ���� �ʱ�ȭ

        _playerInteract.isPlayerInteracting = false;

    }

    public void SetNextDatasID(NPCData _NPCData)        // �������� ���� ����Ʈ ���� �ѱ��
    {
            _NPCData.SetNextID();
    }

    private void MoveToNextDialogueContext()        // ���� ��� �ѱ��
    {
        dialogueIndex++;
    }
}
