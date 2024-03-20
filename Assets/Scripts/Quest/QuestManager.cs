using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    private PlayerInteract _playerInteract;

    [Header("������")]
    public QuestData[] questDatas;
    public QuestConditionData[] questConditionDatas;

    [Header("UI")]
    public GameObject quest_info_panel;
    public TextMeshProUGUI quest_name_tmp;
    public TextMeshProUGUI quest_description_tmp;

    [Header("Ȱ��/��Ȱ�� ������Ʈ")]
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

        SetQuestConditionsOnData();     // ����Ʈ ���� ���� �� �����Ϳ� �Ҵ�
        InitializeQuestDataState();     // ����Ʈ ������ ���� ���� �ʱ�ȭ

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


    public QuestData GetCurQuestData()      // ���� ����Ǵ� ����Ʈ ������ ȣ��
    {
        Debug.Log("���� ����Ʈ �����ʹ� : " + questDatas[questIndex]);
        return questDatas[questIndex];
    }

    public QuestConditionData GetCurQuestCondition()    // ���� ����Ʈ �������� ���� ���� ���
    {
        return questConditionDatas[questIndex];
    }


    public void StartQuest(QuestData data)      // ����Ʈ ���� ó��
    {

        data.questState = QuestData.q_state.progress;

        quest_info_panel.SetActive(true);
        quest_name_tmp.text = data.questName;

        ExcuteOnQuestStart();
        //quest_description_tmp.text = data.questDescription_inprogress;

        // ���Ŀ� ����Ʈ ��ũ���ͺ� �������Ϳ��� UI �ֽ�ȭ�� �����ϴ� ������..
    }

    public void ExcuteOnQuestStart()        // ����Ʈ ���� �� ���� �Լ���, startquest���� ����
    {
        ActiveObjects();
    }

    public void ExcuteOnQuestClear()        // ����Ʈ Ŭ���� �� ���� �Լ���, endtalk���� ����
    {
        UnActiveObjects();
        MoveToNextQuest();
    }

    public void SetNextDialogueIDOnQuestClear()
    {
        _dialogueManager.SetNextDatasID(_playerInteract.scannedObjectHolder.GetComponent<NPCData>());
    }

    void ActiveObjects()     // �� ����Ʈ���� �ʿ��� Ȱ��ȭ ������Ʈ ó��
    {
        activeObjects[questIndex].SetActive(true);
    }

    void UnActiveObjects()   // �� ����Ʈ���� �ʿ� ���� ��Ȱ��ȭ ������Ʈ ó��
    {
        activeObjects[questIndex].SetActive(false);
    }

    public void MoveToNextQuest()   // ���� ����Ʈ�� ����
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
