using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataByIndex", menuName = "Quest/Info")]
public class QuestData : ScriptableObject
{
    public int quesitID;

    public enum q_state
    {
        before,
        progress,
        clear
    }

    public q_state questState;

    public enum q_type
    {
        search,
        getItem,
        eliminate
    }

    public q_type questType;

    public QuestConditionData conditionData;

    public string questName;
    public string[] questDescription_inprogress;
}
