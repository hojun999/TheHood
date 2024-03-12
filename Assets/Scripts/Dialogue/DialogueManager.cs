using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private string _CSVFileName;

    public PlayerInteract _playerInteract;

    public GameObject dialogueUI;

    //public int dialogueID;
    private int dialogueIndex;

    private void Awake()
    {
        DialogueParser dialogueParser = GetComponent<DialogueParser>();
        //DialogueData[] dialogueDatas = dialogueParser.Parse(_CSVFileName);

        //for (int i = 0; i < dialogueDatas.Length; i++)
        //{
        //    // 대화 ID를 키로 사용하여 대화 컨텍스트 배열을 dictionary에 할당합니다.
        //    dialogueDic.Add(dialogueDatas[i].talkID, dialogueDatas[i].dialogueContexts);
        //}

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
