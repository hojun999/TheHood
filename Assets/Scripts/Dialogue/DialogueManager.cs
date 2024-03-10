using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private string _CSVFileName;

    public Dictionary<int, string[]> dialogueDic = new Dictionary<int, string[]>();
    public Dictionary<string, Dictionary<int, string[]>> dialogueBundleDic = new Dictionary<string, Dictionary<int, string[]>>();

    //public Queue<Dictionary<string, Dictionary<int, string[]>>> dialogueBundleQueue = new Queue<Dictionary<string, Dictionary<int, string[]>>>();

    public int dialogueID;
    private int contextIndex;

    private void Awake()
    {
        DialogueParser dialogueParser = GetComponent<DialogueParser>();
        //DialogueData[] dialogueDatas = dialogueParser.Parse(_CSVFileName);

        //for (int i = 0; i < dialogueDatas.Length; i++)
        //{
        //    // ��ȭ ID�� Ű�� ����Ͽ� ��ȭ ���ؽ�Ʈ �迭�� dictionary�� �Ҵ��մϴ�.
        //    dialogueDic.Add(dialogueDatas[i].talkID, dialogueDatas[i].dialogueContexts);
        //}

    }


    public string GetDialogue(int id, int contextIndex)
    {
        if (contextIndex == dialogueDic[id].Length)
        {
            return null;
        }
        else
        {
            return dialogueDic[id][contextIndex];
        }
    }

    private void Start()
    {
        foreach (var pair in dialogueDic)
        {
            Debug.Log("Dialogue ID: " + pair.Key);
            Debug.Log("Dialogue Contexts: ");
            foreach (var context in pair.Value)
            {
                Debug.Log(" - " + context);
            }
        }

    }
}
