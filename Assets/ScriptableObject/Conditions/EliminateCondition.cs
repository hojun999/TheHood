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

    public int eliminatedNum;
    //private int addedNeedNum;

    public override bool IsAchieved()
    {
        bool isAchieved = false;

        int enemyTypeNum = enemyCondition.Length;

        if(enemyTypeNum >= 2)       // 퀘스트4 조건 처리 
        {
            for (int i = 0; i < enemyTypeNum; i++)
            {
                enemyConditionDic.Add(enemyCondition[i].enemyType, enemyCondition[i].needNum);
                //addedNeedNum += enemyCondition[i].needNum;
            }


            if (enemyConditionDic.ContainsKey(EnemyCondition.e_type.boss))
            {
                int eliminatedBossNum = 1;

                if (enemyConditionDic[EnemyCondition.e_type.boss] == eliminatedBossNum)
                {
                    isAchieved = true;
                }
            }
            else if (enemyConditionDic.ContainsKey(EnemyCondition.e_type.normal))
            {
                int eliminatedNormalNum = 6;

                if (enemyConditionDic[EnemyCondition.e_type.normal] == eliminatedNormalNum)
                {
                    isAchieved = true;
                }
            }
            else
                isAchieved = false;
        }
        else        // 퀘스트3 조건 처리
        {
            isAchieved = false;

            if (eliminatedNum >= enemyCondition[0].needNum)
                isAchieved = true;
        }

        return isAchieved;

    }
}
