using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string npcName;

    public string[] dialogueContexts;
}

public class DialogueAction
{
    public string questNameByDialogue;

    public Vector2 line;
    public DialogueData[] dialogueDatas;
}
