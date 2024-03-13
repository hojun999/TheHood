using System;
using System.Collections;
using System.Collections.Generic;
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
        PrintDialogueBundle(i_dialogueBundleByNPCName);
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
                Debug.Log(npcName);

                _NPCdatas[i].m_dialogueDic = dialogueBundleByNPCName[npcName];
            }
            else
            {
                Debug.Log("npcName�� �´� ������ ����");
            }
        }
    }


    public void SetTalkDataOnInteract(GameObject scanObj)
    {
        NPCData _NPCData = scanObj.GetComponent<NPCData>();
        Talk(_NPCData, _NPCData.m_dialogueID, _NPCData.NPCType);
        dialogueUI.SetActive(_playerInteract.isPlayerInteracting);
    }

    private void Talk(NPCData data,int ID, Enum type)
    {
        string dialogueContext = data.m_dialogueDic[ID][dialogueIndex];
    }

    private string GetTalk(NPCData data, int ID)
    {
        NPCData.m_type npcType = data.NPCType;
        string context = data.m_dialogueDic[ID][dialogueIndex];

        switch (npcType)
        {
            case NPCData.m_type.client:
                if (context != null)
                {
                    dialogueIndex++;
                    return context;
                }
                else
                {
                    EndTalk();
                    return null;
                }
                break;
            case NPCData.m_type.trador:
                if (true)   // �κ�manager�� ���� �� ������ ���� ������ ���� ��ȭ �ٸ��� �ϴ� ��� �ۼ�
                {
                    if(context != null)
                    {
                        //TradeEvent();
                        dialogueID++;
                        return context;
                    }
                    else
                    {
                        EndTalk();
                        return null;
                    }
                }
                break;
        }

    }

    private void EndTalk()
    {
        dialogueIndex = 0;
        //_questManager.CheckQuestClear();
    }
}
