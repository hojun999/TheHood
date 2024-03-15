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

        for (int i = 0; i < _unactive_group.Length; i++)        // �� �迭�� ������ �ٸ� �� �����Ƿ� �и��Ͽ� for��
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

    public void CheckQuestClear(QuestData_New data)
    {
        //if (data.questState)
        //    ExcuteOnQuestClear(data);
        if (data.questState == QuestData_New.q_state.clear)
            ExcuteOnQuestClear();
    }

    void ActiveObjectsByQuestProgress()
    {
            foreach (GameObject obj in activeObjectsDic[questIndex])     // ������ ������ GameObject[]�� �ƴ� GameObject�� �־��� ������, ���������� ����ϴ��� Ȯ�� �ʿ�
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
