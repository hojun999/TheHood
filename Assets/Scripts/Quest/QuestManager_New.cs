using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager_New : MonoBehaviour
{
    private DialogueManager _dialogueManager;

    public QuestData_New[] questDatas;

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

        for (int i = 0; i < _active_group.Length; i++)
        {
            activeObjectsDic.Add(i + 1, _active_group[i].active_group);
        }

        for (int i = 0; i < _unactive_group.Length; i++)        // 두 배열의 개수가 다를 수 있으므로 분리하여 for문
        {
            unactiveObjectsDic.Add(i + 1, _unactive_group[i].unactive_group);
        }

    }

    public QuestData_New GetCurQuestData()
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

        _dialogueManager.MoveToNextDialogueID();
        MoveToNextQuest();
    }

    public void CheckQuestState()
    {
        QuestData_New data = GetCurQuestData();

        switch (data.questState)
        {
            case QuestData_New.q_state.before:
                StartQuest(data);
                break;
            case QuestData_New.q_state.progress:
                break;
            case QuestData_New.q_state.clear:
                ExcuteOnQuestClear();
                break;
        }
    }

    public void StartQuest(QuestData_New data)
    {
        data.questState = QuestData_New.q_state.progress;
        Debug.Log(data.questState);

        quest_info_panel.SetActive(true);
        quest_name_tmp.text = data.questName;
        //quest_description_tmp.text = data.questDescription_inprogress;
        // 이후에 퀘스트 스크립터블 오브젝터에서 UI 최신화도 구현하는 것으로..
    }

    void ActiveObjectsByQuestProgress()
    {
            foreach (GameObject obj in activeObjectsDic[questIndex])     // 데이터 형식을 GameObject[]가 아닌 GameObject로 넣었기 때문에, 정상적으로 기능하는지 확인 필요
            {
                obj.SetActive(true);
            }

    }

    void UnActiveObjectsByQuestProgress()
    {
        foreach (GameObject obj in unactiveObjectsDic[questIndex])
        {
            obj.SetActive(false);
        }
    }

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
