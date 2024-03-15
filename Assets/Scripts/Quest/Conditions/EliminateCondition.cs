using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyCondition
{
    public enum e_type
    {
        boss,
        normal
    }

    public e_type enemyType;

    public int needNum;
}

[CreateAssetMenu(menuName = ("Quest/Conditions/EliminateCondition"))]
public class EliminateCondition : QuestCondition
{

    public EnemyCondition[] enemyCondition;

    private Dictionary<EnemyCondition.e_type, int> enemyConditionDic = new Dictionary<EnemyCondition.e_type, int>();

    public int achievedNum;
    private int addedNeedNum;

    public override bool IsAchieved()
    {
        bool isAchieved = false;

        int enemyTypeNum = enemyCondition.Length;

        if(enemyTypeNum >= 2)
        {
            for (int i = 0; i < enemyTypeNum; i++)
            {
                enemyConditionDic.Add(enemyCondition[i].enemyType, enemyCondition[i].needNum);
                addedNeedNum += enemyCondition[i].needNum;
            }

            if (achievedNum >= addedNeedNum)
                isAchieved = true;

            if (enemyConditionDic.ContainsKey(EnemyCondition.e_type.boss))
            {
                int individualNeedNum = 1;

                if (enemyConditionDic[EnemyCondition.e_type.boss] == individualNeedNum)
                {
                    // 텍스트 최신화 작성
                }
            }
        }
        else
        {
            if (achievedNum >= enemyCondition[0].needNum)
                isAchieved = true;
        }

        return isAchieved;

    }
}
