using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        string[] data = csvData.text.Split(new char[]{'\n'}); // 엔터 기준 분리

        for (int i = 1; i < data.Length;)
        {
            Debug.Log(data[i]);

            if(++i < data.Length)
            {
                ;
            }
        }

        return dialogueList.ToArray();
    }

    private void Start()
    {
        Parse("TalkData");
    }
}
