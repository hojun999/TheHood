using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public PlayerInteract _playerInteract;
    private DialogueParserTest _dialougeParserTest;

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
