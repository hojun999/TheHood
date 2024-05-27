using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBoss : EnemyEntity
{
    Quest4Object quest4Object;

    Camera mainCamera;

    public GameObject quest4ColliderGroup;
    public TextMeshProUGUI questDiscription_line1;

    private void Start()
    {
        gameObject.GetComponent<EnemyBoss>().enabled = false;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        quest4Object = gameObject.GetComponent<Quest4Object>();
    }

    public override void MoveRandomDirection()
    {
        xMove = Random.Range(-1f, 1f);
        yMove = Random.Range(-1f, 1f);
    }

    public override void RefreshQuestConditionByProgress()
    {   
        Quest4Condition quest4Condition = (Quest4Condition)questManager.questConditionDatas[3];
        quest4Condition.getBossCount++;
   
        questDiscription_line1.text = "º¸½º " + quest4Condition.getBossCount + "/ 1";

        if(quest4Condition.getNormalEnemyCount == 6 && quest4Condition.getBossCount == 1)
        {
            mainCamera.GetComponent<CameraController>().enabled = true;
            quest4Object.ActionOnClear();
            quest4ColliderGroup.SetActive(false);
            questManager.AddStrikethroughOnTMP(questDiscription_line1);
            questManager.ConvertColorRedOnTmp(questDiscription_line1);
        }
        else if(quest4Condition.getBossCount == 1)
        {
            questManager.AddStrikethroughOnTMP(questDiscription_line1);
            questManager.ConvertColorRedOnTmp(questDiscription_line1);
        }
    }
}
