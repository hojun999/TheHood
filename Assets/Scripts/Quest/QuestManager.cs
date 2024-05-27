using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    // ����
    #region
    private GameManager _gamaManager;
    private DialogueManager _dialogueManager;
    private PlayerInteract _playerInteract;

    [Header("������")]
    public QuestData[] questDatas;
    public QuestConditionData[] questConditionDatas;

    [Header("UI")]
    public GameObject quest_info_panel;
    public GameObject quest_clear_UI;
    public TextMeshProUGUI quest_name;
    public TextMeshProUGUI quest_description_line1;
    public TextMeshProUGUI quest_description_line2;
    public TextMeshProUGUI quest_description_line3;

    [Header("Ȱ��/��Ȱ�� ������Ʈ")]
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

        SetQuestConditionsOnData();     // ����Ʈ ���� ���� �� �����Ϳ� �Ҵ�
        InitializeQuestDataState();     // ����Ʈ ������ ���� ���� �ʱ�ȭ
        InitializeQuestConditionData(); // ����Ʈ ���� ���� �ʱ�ȭ
    }


    private void SetQuestConditionsOnData()     // ����Ʈ ������ �� ����Ʈ �����Ϳ� �Ҵ�
    {
        for (int i = 0; i < questConditionDatas.Length; i++)
        {
            questDatas[i].conditionData = questConditionDatas[i];
        }
    }

    private void InitializeQuestDataState()     // ����Ʈ �������� ���� ���� before�� �ʱ�ȭ
    {
        foreach (QuestData questData in questDatas)
            questData.questState = QuestData.q_state.before;
    }

    private void InitializeQuestConditionData() // ���� ��, �� ����Ʈ ������ ������ �ʱ�ȭ
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

    public QuestData GetCurQuestData()      // ���� ����Ǵ� ����Ʈ ������ ȣ��
    {
        Debug.Log("���� ����Ʈ �����ʹ� : " + questDatas[questIndex]);
        return questDatas[questIndex];
    }

    public QuestData GetBeforeQuestData()
    {
        if (questIndex > 0)
            return questDatas[questIndex - 1];
        else
            return null;
    }

    public QuestConditionData GetCurQuestCondition()    // ���� ����Ʈ �������� ���� ���� ���
    {
        return questConditionDatas[questIndex];
    }

    public void StartQuest(QuestData data)      // ����Ʈ ���� ó��, ��ȭ�� ���� �� �Ǵ��Ͽ� ȣ��
    {

        data.questState = QuestData.q_state.progress;

        quest_info_panel.SetActive(true);
        quest_name.text = data.questName;
        SetQuestDiscriptionText(data);

        ExcuteOnQuestStart();
    }

    void SetQuestDiscriptionText(QuestData data)    // �� ����Ʈ�� ������ �޼����� ��, �޼��� ���ǿ� ���� ����Ʈ ���� ���� UI ��ȯ
    {
        InitializeDescriptionText();       // ����Ʈ UI �ؽ�Ʈ ���� �Ͼ������ �ʱ�ȭ

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

    public void ExcuteOnQuestStart()        // ����Ʈ ���� �� ���� �Լ���, startquest���� ����
    {
        ActiveObjects();
        UnActiveObjects();
    }

    public void ExcuteOnQuestClear()        // ����Ʈ Ŭ���� �� ���� �Լ���, endtalk���� ����
    {
        if (CheckEnding() && !_gamaManager.isEnding)
            _gamaManager.EndingSequence();
    }

    public void SetNextDialogueIDOnQuestClear()     // ����Ʈ ���� �޼� �� ���� ��ȭ�� �ҷ����� ����
    {
        _dialogueManager.SetNextDatasIDOnClient();
    }

    void ActiveObjects()     // �� ����Ʈ���� �ʿ��� Ȱ��ȭ ������Ʈ ó��
    {
        activeObjects[questIndex].SetActive(true);
    }

    void UnActiveObjects()   // �� ����Ʈ���� �ʿ� ���� ��Ȱ��ȭ ������Ʈ ó��
    {
        if(questIndex > 0)
            activeObjects[questIndex - 1].SetActive(false);
    }

    public void MoveToNextQuest()   // ���� ����Ʈ�� ����
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
