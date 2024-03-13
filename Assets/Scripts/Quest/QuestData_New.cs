using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataByIndex", menuName = "Scriptable object/Quest")]
public class QuestData_New : ScriptableObject
{
    public int quesitID;

    public enum q_Type
    {
        Search,
        getItem,
        Eliminate
    }

    public q_Type questType;

    public string questDescription_inprogress;
    public string questDescription_achieve;

    public bool isCurQeustClear;
}
