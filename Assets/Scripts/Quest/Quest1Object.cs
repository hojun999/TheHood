using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest1Object : QuestObject
{
    public Quest1Condition questCondition;

    public TextMeshProUGUI questDiscription_line1;
    public TextMeshProUGUI questDiscription_line2;

    private void Start()
    {
        SetNeedData();
    }

    public override void HandleCondition()
    {
        switch (gameObject.tag)
        {
            case "BigArea":
                questCondition.checkBigArea = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                questManager.AddStrikethroughOnTMP(questDiscription_line1);
                questManager.ConvertColorRedOnTmp(questDiscription_line1);
                break;
            case "SmallArea":
                questCondition.checkSmallArea = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                questManager.AddStrikethroughOnTMP(questDiscription_line2);
                questManager.ConvertColorRedOnTmp(questDiscription_line2);
                break;
        }

        CheckClear();
    }

    public override void CheckClear()
    {
        if (questCondition.checkBigArea && questCondition.checkSmallArea)
            ActionOnClear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleCondition();
        }
    }


}
