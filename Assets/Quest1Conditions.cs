using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1Conditions : MonoBehaviour
{
    QuestManager _questManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);


            _questManager.GetCurQuestData().questState = QuestData.q_state.clear;
        }
    }
}