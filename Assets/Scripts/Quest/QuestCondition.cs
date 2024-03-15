using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCondition : ScriptableObject
{
    public virtual bool IsAchieved()
    {
        return true;
    }
}
