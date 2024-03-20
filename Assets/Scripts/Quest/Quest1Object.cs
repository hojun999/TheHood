using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1Object : QuestObject
{
    public Quest1Condition questCondition;

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
                break;
            case "SmallArea":
                questCondition.checkSmallArea = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            }

        CheckClear();
    }

    public override void CheckClear()
    {
        if (questCondition.checkBigArea && questCondition.checkSmallArea)
        {
            questManager.MoveToNextQuest();
            SetNextDialogueID();
            ActiveQuestionMarkOnClear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleCondition();
        }
    }


}
