using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyNormal : EnemyEntity
{
    Quest3Object quest3Object;
    Quest4Object quest4Object;

    Camera mainCamera;

    public GameObject quest4ColliderGroup;

    public TextMeshProUGUI questDiscription_line1;

    private void Awake()
    {
        gameObject.GetComponent<EnemyNormal>().enabled = false;
    }

    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        switch (questManager.questIndex)
        {
            case 2:
                quest3Object = gameObject.GetComponent<Quest3Object>();
                break;
            case 3:
                quest4Object = gameObject.GetComponent<Quest4Object>();
                break;
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public override void MoveRandomDirection()
    {
        xMove = Random.Range(-0.8f, 0.8f);
        yMove = Random.Range(-0.8f, 0.8f);
    }

    public override void RefreshQuestConditionByProgress()
    {
        switch (questManager.questIndex)
        {
            case 2:
                Quest3Condition quest3Condition = (Quest3Condition)questManager.questConditionDatas[2];
                quest3Condition.getNormalEnemyCount++;
                questDiscription_line1.text = "숲에 흩어져있는 부하 " + quest3Condition.getNormalEnemyCount + "/ 5";

                if (quest3Condition.getNormalEnemyCount == 5)
                {
                    questManager.AddStrikethroughOnTMP(questDiscription_line1);
                    questManager.ConvertColorRedOnTmp(questDiscription_line1);
                    quest3Object.ActionOnClear();
                }
                break;
            case 3:
                Quest4Condition quest4Condition = (Quest4Condition)questManager.questConditionDatas[3];
                quest4Condition.getNormalEnemyCount++;
                questDiscription_line1.text = "부하 " + quest4Condition.getNormalEnemyCount + "/ 6";

                if (quest4Condition.getNormalEnemyCount == 6 && quest4Condition.getBossCount == 1)
                {
                    mainCamera.GetComponent<CameraController>().enabled = true;
                    quest4Object.ActionOnClear();
                    quest4ColliderGroup.SetActive(false);
                    questManager.AddStrikethroughOnTMP(questDiscription_line1);
                    questManager.ConvertColorRedOnTmp(questDiscription_line1);
                }
                else if (quest4Condition.getNormalEnemyCount == 6)
                {
                    questManager.AddStrikethroughOnTMP(questDiscription_line1);
                    questManager.ConvertColorRedOnTmp(questDiscription_line1);
                }
                break;
        }
    }
}
