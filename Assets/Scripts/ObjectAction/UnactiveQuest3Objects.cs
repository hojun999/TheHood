using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnactiveQuest3Objects : MonoBehaviour
{
    public QuestManager _questManager;

    private void OnEnable()
    {
        _questManager.unactiveObjects[2].SetActive(false);
    }
}
