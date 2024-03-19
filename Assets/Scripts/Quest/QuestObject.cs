using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public QuestManager questManager;
    public QuestData questData;

    public void SetNeedData()
    {
        questData = questManager.GetCurQuestData();
    }

    public virtual void HandleCondition()
    {

    }

    public virtual void CheckClear()
    {

    }

    public void SetNextDialogueID()
    {
        questManager.SetNextDialogueIDOnQuestClear();
    }
}
