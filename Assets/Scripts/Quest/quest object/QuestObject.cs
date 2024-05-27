using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestObject : MonoBehaviour
{
    public QuestManager questManager;
    private QuestData questData;

    public void SetNeedData()
    {
        questData = questManager.GetCurQuestData();
    }

    public virtual void HandleCondition() { }

    public virtual void CheckClear() { }

    public void ActiveQuestionMarkOnClear()
    {
        questManager.questionmark.SetActive(true);
    }

    public void ActionOnClear()
    {
        questManager.InstanciateQuestClearText();
        questManager.MoveToNextQuest();
        questManager.SetNextDialogueIDOnQuestClear();
        ActiveQuestionMarkOnClear();
    }

}
