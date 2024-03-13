using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager_New : MonoBehaviour
{
    private DialogueManager _dialogueManager;

    public QuestData_New[] questDatas;

    public GameObject quest_info_panel;
    public TextMeshProUGUI quest_description_tmp;

    [SerializeField] private SetActiveObjectGroup[] _active_group;
    [SerializeField] private SetUnactiveObjectGroup[] _unactive_group;

    private Dictionary<int, GameObject[]> activeActionDic = new Dictionary<int, GameObject[]>();
    private Dictionary<int, GameObject[]> unactiveActionDic = new Dictionary<int, GameObject[]>();

    private int questIndex;

    private void Start()
    {
        _dialogueManager = gameObject.GetComponent<DialogueManager>();

        for (int i = 0; i < _active_group.Length; i++)
        {
            activeActionDic.Add(i + 1, _active_group[i].active_group);
        }

        for (int i = 0; i < _unactive_group.Length; i++)        // �� �迭�� ������ �ٸ� �� ����
        {
            unactiveActionDic.Add(i + 1, _unactive_group[i].unactive_group);
        }

    }

    public void ExcuteOnQuestStart(QuestData_New data)
    {
        switch (data.quesitID)
        {
            case 1:
                break;
        }
    }

    public void CheckQuestClear(QuestData_New data)
    {
        if (data.isCurQeustClear)
            ExcuteOnQuestClear(data);
    }

    public void ExcuteOnQuestClear(QuestData_New data)
    {
        _dialogueManager.dialogueID++;

        switch(data.quesitID)
        {
            case 1:
                break;
        }
    }

    void ActiveObjectsByQuestProgress()
    {
        foreach (GameObject obj in activeActionDic[questIndex])     // ������ ������ GameObject[]�� �ƴ� GameObject�� �־��� ������, ���������� ����ϴ��� Ȯ�� �ʿ�
        {
            obj.SetActive(true);
        }
    }

    void UnActiveObjectsByQuestProgress()
    {
        foreach (GameObject obj in unactiveActionDic[questIndex])
        {
            obj.SetActive(false);
        }
    }

}
