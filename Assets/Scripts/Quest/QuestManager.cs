using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    // 변수
    #region
    private GameManager _gamaManager;
    private DialogueManager _dialogueManager;
    private PlayerInteract _playerInteract;

    [Header("데이터")]
    public QuestData[] questDatas;
    public QuestConditionData[] questConditionDatas;

    [Header("UI")]
    public GameObject quest_info_panel;
    public GameObject quest_clear_UI;
    public TextMeshProUGUI quest_name;
    public TextMeshProUGUI quest_description_line1;
    public TextMeshProUGUI quest_description_line2;
    public TextMeshProUGUI quest_description_line3;

    [Header("활성/비활성 오브젝트")]
    public GameObject questionmark;
    public GameObject exclamationmark;

    public GameObject[] activeObjects;
    public GameObject[] unactiveObjects;

    public Dictionary<int, GameObject> activeObjectsDic = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> unactiveObjectsDic = new Dictionary<int, GameObject>();

    public int questIndex;
    [HideInInspector] public bool canStartQuest;
    #endregion

    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        _gamaManager = gameObject.GetComponent<GameManager>();
        _dialogueManager = gameObject.GetComponent<DialogueManager>();
        _playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();

        SetQuestConditionsOnData();     // 퀘스트 조건 정보 각 데이터에 할당
        InitializeQuestDataState();     // 퀘스트 데이터 진행 상태 초기화
        InitializeQuestConditionData(); // 퀘스트 조건 정보 초기화
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

    private void InitializeQuestConditionData() // 시작 시, 각 퀘스트 조건의 데이터 초기화
    {
        if(questConditionDatas[0] is Quest1Condition)
        {
            Quest1Condition quest1Condition = (Quest1Condition)questConditionDatas[0];
            quest1Condition.checkBigArea = false;
            quest1Condition.checkSmallArea = false;
        }

        if(questConditionDatas[1] is Quest2Condition)
        {
            Quest2Condition quest2Condition = (Quest2Condition)questConditionDatas[1];
            quest2Condition.getItemCount = 0;
        }

        if(questConditionDatas[2] is Quest3Condition)
        {
            Quest3Condition quest3Condition = (Quest3Condition)questConditionDatas[2];
            quest3Condition.getNormalEnemyCount = 0;
        }

        if(questConditionDatas[3] is Quest4Condition)
        {
            Quest4Condition quest4Condition = (Quest4Condition)questConditionDatas[3];
            quest4Condition.getBossCount = 0;
            quest4Condition.getNormalEnemyCount = 0;
        }

    }

    public QuestData GetCurQuestData()      // 현재 진행되는 퀘스트 데이터 호출
    {
        Debug.Log("현재 퀘스트 데이터는 : " + questDatas[questIndex]);
        return questDatas[questIndex];
    }

    public QuestData GetBeforeQuestData()
    {
        if (questIndex > 0)
            return questDatas[questIndex - 1];
        else
            return null;
    }

    public QuestConditionData GetCurQuestCondition()    // 현재 퀘스트 데이터의 조건 정보 얻기
    {
        return questConditionDatas[questIndex];
    }

    public void StartQuest(QuestData data)      // 퀘스트 시작 처리, 대화가 끝날 때 판단하여 호출
    {

        data.questState = QuestData.q_state.progress;

        quest_info_panel.SetActive(true);
        quest_name.text = data.questName;
        SetQuestDiscriptionText(data);

        ExcuteOnQuestStart();
    }

    void SetQuestDiscriptionText(QuestData data)    // 각 퀘스트의 조건을 달성했을 때, 달성한 조건에 대한 퀘스트 진행 정보 UI 전환
    {
        InitializeDescriptionText();       // 퀘스트 UI 텍스트 색깔 하얀색으로 초기화

        switch (data.quesitID)
        {
            case 1:
                quest_description_line1.gameObject.SetActive(true);
                quest_description_line1.text = data.questDescription_inprogress[0];

                quest_description_line2.gameObject.SetActive(true);
                quest_description_line2.text = data.questDescription_inprogress[1];

                quest_description_line3.gameObject.SetActive(false);
                break;
            case 2:
                quest_description_line1.gameObject.SetActive(true);
                quest_description_line1.text = data.questDescription_inprogress[0];

                quest_description_line2.gameObject.SetActive(true);
                quest_description_line2.text = data.questDescription_inprogress[1];

                quest_description_line3.gameObject.SetActive(true);
                quest_description_line3.text = data.questDescription_inprogress[2];
                break;
            case 3:
                quest_description_line1.gameObject.SetActive(true);
                quest_description_line1.text = data.questDescription_inprogress[0];

                quest_description_line2.gameObject.SetActive(false);
                quest_description_line3.gameObject.SetActive(false);
                break;
            case 4:
                quest_description_line1.gameObject.SetActive(true);
                quest_description_line1.text = data.questDescription_inprogress[0];

                quest_description_line2.gameObject.SetActive(true);
                quest_description_line2.text = data.questDescription_inprogress[1];

                quest_description_line3.gameObject.SetActive(false);
                break;
        }
    }   

    void InitializeDescriptionText()
    {
        quest_description_line1.color = Color.white;
        quest_description_line2.color = Color.white;
        quest_description_line3.color = Color.white;

        RemoveStrikethoroughOnTMP(quest_description_line1);
        RemoveStrikethoroughOnTMP(quest_description_line2);
        RemoveStrikethoroughOnTMP(quest_description_line3);
    }

    public void AddStrikethroughOnTMP(TextMeshProUGUI textComponent)
    {
        textComponent.text = "<s>" + textComponent.text + "</s>";
    }

    public void RemoveStrikethoroughOnTMP(TextMeshProUGUI textComponent)
    {
        textComponent.text = textComponent.text.Replace("<s>", "").Replace("</s>", "");
    }

    public void ConvertColorRedOnTmp(TextMeshProUGUI textComponent)
    {
        textComponent.color = Color.red;
    }

    public void ExcuteOnQuestStart()        // 퀘스트 시작 시 실행 함수들, startquest에서 실행
    {
        ActiveObjects();
        UnActiveObjects();
    }

    public void ExcuteOnQuestClear()        // 퀘스트 클리어 시 실행 함수들, endtalk에서 실행
    {
        if (CheckEnding() && !_gamaManager.isEnding)
            _gamaManager.EndingSequence();
    }

    public void SetNextDialogueIDOnQuestClear()     // 퀘스트 조건 달성 시 다음 대화를 불러오기 위함
    {
        _dialogueManager.SetNextDatasIDOnClient();
    }

    void ActiveObjects()     // 각 퀘스트마다 필요한 활성화 오브젝트 처리
    {
        activeObjects[questIndex].SetActive(true);
    }

    void UnActiveObjects()   // 각 퀘스트마다 필요 없는 비활성화 오브젝트 처리
    {
        if(questIndex > 0)
            activeObjects[questIndex - 1].SetActive(false);
    }

    public void MoveToNextQuest()   // 다음 퀘스트로 진행
    {
        questIndex++;
    }

    public void InstanciateQuestClearText()
    {
        StartCoroutine(coInstanciateQuestClearText());
    }

    IEnumerator coInstanciateQuestClearText()
    {
        GameObject clearUI = Instantiate(quest_clear_UI, GameObject.Find("MainUICanvas").transform);
        yield return new WaitForSeconds(2f);
        Destroy(clearUI);
    }

    bool CheckEnding()
    {
        if (questDatas[4].questState == QuestData.q_state.progress)
            return true;
        else
            return false;
    }
}
