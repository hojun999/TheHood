using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    private PlayerInteract _playerInteract;

    [Header("데이터")]
    public QuestData[] questDatas;
    public QuestConditionData[] questConditionDatas;

    [Header("UI")]
    public GameObject quest_info_panel;
    public TextMeshProUGUI quest_name_tmp;
    public TextMeshProUGUI quest_description_tmp;

    [Header("활성/비활성 오브젝트")]
    public GameObject questionmark;
    public GameObject exclamationmark;

    [SerializeField] private GameObject[] activeObjects;
    [SerializeField] private GameObject[] unactiveObjects;

    public Dictionary<int, GameObject> activeObjectsDic = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> unactiveObjectsDic = new Dictionary<int, GameObject>();

    [SerializeField] private int questIndex;
    private int i_relativeDialogueID;

    

    [HideInInspector] public bool canStartQuest;

    private void Start()
    {
        _dialogueManager = gameObject.GetComponent<DialogueManager>();
        _playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();

        SetQuestConditionsOnData();     // 퀘스트 조건 정보 각 데이터에 할당
        InitializeQuestDataState();     // 퀘스트 데이터 진행 상태 초기화

    }

    private void SetQuestConditionsOnData()     // 퀘스트 조건을 각 퀘스트 데이터에 할당
    {
        for (int i = 0; i < questConditionDatas.Length; i++)
        {
            questDatas[i].conditionData = questConditionDatas[i];
        }
    }

    private void InitializeQuestDataState()     // 퀘스트 데이터의 진행 상태 before로 초기화
    {
        foreach (QuestData questData in questDatas)
            questData.questState = QuestData.q_state.before;
    }


    public QuestData GetCurQuestData()      // 현재 진행되는 퀘스트 데이터 호출
    {
        Debug.Log("현재 퀘스트 데이터는 : " + questDatas[questIndex]);
        return questDatas[questIndex];
    }

    public QuestConditionData GetCurQuestCondition()    // 현재 퀘스트 데이터의 조건 정보 얻기
    {
        return questConditionDatas[questIndex];
    }


    public void StartQuest(QuestData data)      // 퀘스트 시작 처리
    {

        data.questState = QuestData.q_state.progress;

        quest_info_panel.SetActive(true);
        quest_name_tmp.text = data.questName;

        ExcuteOnQuestStart();
        //quest_description_tmp.text = data.questDescription_inprogress;

        // 이후에 퀘스트 스크립터블 오브젝터에서 UI 최신화도 구현하는 것으로..
    }

    public void ExcuteOnQuestStart()        // 퀘스트 시작 시 실행 함수들, startquest에서 실행
    {
        ActiveObjects();
    }

    public void ExcuteOnQuestClear()        // 퀘스트 클리어 시 실행 함수들, endtalk에서 실행
    {
        UnActiveObjects();
        MoveToNextQuest();
    }

    public void SetNextDialogueIDOnQuestClear()
    {
        _dialogueManager.SetNextDatasID(_playerInteract.scannedObjectHolder.GetComponent<NPCData>());
    }

    void ActiveObjects()     // 각 퀘스트마다 필요한 활성화 오브젝트 처리
    {
        activeObjects[questIndex].SetActive(true);
    }

    void UnActiveObjects()   // 각 퀘스트마다 필요 없는 비활성화 오브젝트 처리
    {
        activeObjects[questIndex].SetActive(false);
    }

    public void MoveToNextQuest()   // 다음 퀘스트로 진행
    {
        questIndex++;
    }

    //void EndingEvent()
    //{
    //    StartCoroutine(coEndingEvent());
    //}

    //IEnumerator coEndingEvent()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //}
}
