using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyEntity
{
    public override void MoveRandomDirection()
    {
        xMove = Random.Range(-1f, 1f);
        yMove = Random.Range(-1f, 1f);
    }

    public override void RefreshQuestConditionByProgress()
    {
        if (questManager.questIndex == 4)
        {
            Quest4Condition quest4Condition = (Quest4Condition)questManager.questConditionDatas[3];
            quest4Condition.getBossCount++;
        }
    }
}
