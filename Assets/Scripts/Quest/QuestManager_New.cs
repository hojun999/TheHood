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

    private int questIndex;

    private void Start()
    {
        _dialogueManager = gameObject.GetComponent<DialogueManager>();
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


}
