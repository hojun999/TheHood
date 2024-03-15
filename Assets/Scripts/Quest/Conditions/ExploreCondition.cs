using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Conditions/ExploreCondition")]
public class ExploreCondition : QuestCondition
{
    public int requiredNum;
    public int achievedNum;

    public override bool IsAchieved()
    {
        return achievedNum >= requiredNum;
    }
}
