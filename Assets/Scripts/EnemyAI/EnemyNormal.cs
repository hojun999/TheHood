using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyEntity
{
    public override void MoveRandomDirection()
    {
        xMove = Random.Range(-0.8f, 0.8f);
        yMove = Random.Range(-0.8f, 0.8f);
    }

    public override void RefreshQuestConditionByProgress()
    {
        switch (questManager.questIndex)
        {
            case 3:
                Quest3Condition quest3Condition = (Quest3Condition)questManager.questConditionDatas[2];
                quest3Condition.getNormalEnemyCount++;
                break;
            case 4:
                Quest4Condition quest4Condition = (Quest4Condition)questManager.questConditionDatas[3];
                quest4Condition.getNormalEnemyCount++;
                break;
        }
    }
}
