using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    private DialogueManager _dialogueManager;

    public QuestData[] questDatas;

    public GameObject quest_info_panel;
    public TextMeshProUGUI quest_name_tmp;
    public TextMeshProUGUI quest_description_tmp;

    [SerializeField] private SetActiveObjectGroup[] _active_group;
    [SerializeField] private SetUnactiveObjectGroup[] _unactive_group;

    private Dictionary<int, GameObject[]> activeObjectsDic = new Dictionary<int, GameObject[]>();
    private Dictionary<int, GameObject[]> unactiveObjectsDic = new Dictionary<int, GameObject[]>();

    private int questIndex;
    private int i_relativeDialogueID;

    

    [HideInInspector] public bool canStartQuest;

    private void Start()
    {
        _dialogueManager = gameObject.GetComponent<DialogueManager>();

        // questdata scriptableobejct ���� ���� �ʱ�ȭ
        foreach (QuestData questData in questDatas)
            questData.questState = QuestData.q_state.before;

        // �Ʒ� �迭�� ������ �ٸ� �� �����Ƿ� �и��Ͽ� for��
        for (int i = 0; i < _active_group.Length; i++)
            activeObjectsDic.Add(i + 1, _active_group[i].active_group);

        for (int i = 0; i < _unactive_group.Length; i++)        
            unactiveObjectsDic.Add(i + 1, _unactive_group[i].unactive_group);

    }

    public QuestData GetCurQuestData()
    {
        return questDatas[questIndex];
    }

    public void ExcuteOnQuestStart()
    {
        ActiveObjectsByQuestProgress();
    }

    public void ExcuteOnQuestClear()
    {
        UnActiveObjectsByQuestProgress();

        //_dialogueManager.MoveToNextDialogueID();
        MoveToNextQuest();
    }

    public void SetQuestDataByQuestState(QuestData data)        // �� ����Ʈ ���¿� ���� ó��
    {
        switch (data.questState)
        {
            case QuestData.q_state.before:
                StartQuest(data);       // ����Ʈ ����
                break;
            case QuestData.q_state.progress:
                break;
            case QuestData.q_state.clear:
                ExcuteOnQuestClear();   // ����Ʈ Ŭ���� ���� ó�� �Լ� ȣ��
                break;
        }
    }

    public void StartQuest(QuestData data)
    {
        data.questState = QuestData.q_state.progress;

        quest_info_panel.SetActive(true);
        quest_name_tmp.text = data.questName;
        //quest_description_tmp.text = data.questDescription_inprogress;

        // ���Ŀ� ����Ʈ ��ũ���ͺ� �������Ϳ��� UI �ֽ�ȭ�� �����ϴ� ������..
    }


    void ActiveObjectsByQuestProgress()     // �� ����Ʈ���� �ʿ��� Ȱ��ȭ ������Ʈ ó��
    {
            foreach (GameObject obj in activeObjectsDic[questIndex])     // ������ ������ GameObject[]�� �ƴ� GameObject�� �־��� ������, ���������� ����ϴ��� Ȯ�� �ʿ�
            {
                obj.SetActive(true);
            }

    }

    void UnActiveObjectsByQuestProgress()   // �� ����Ʈ���� �ʿ� ���� ��Ȱ��ȭ ������Ʈ ó��
    {
        foreach (GameObject obj in unactiveObjectsDic[questIndex])
        {
            obj.SetActive(false);
        }
    }

    //void MoveToProgressDialogue()
    //{
    //    GetCurQuestData().cur + 10;
    //}

    void MoveToNextQuest()
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
