using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string m_name;

    public enum m_type
    {
        client,
        trador
    }
    public m_type NPCType;
 
    //public string[] m_contextsArray;
    public Dictionary<int, List<string>> m_dialogueDic = new Dictionary<int, List<string>>();

    [HideInInspector] public int m_dialogueID;

    private void Start()
    {
        PrintAllDialogues();
    }
    public void PrintAllDialogues()
    {
        StartCoroutine(coPrintAllDialogues());
    }

    IEnumerator coPrintAllDialogues()       // ���������� �Ľ��� ������, �޼��� ȣ���� ���ļ� �ܼ�â�� �Ⱥ��̴� �̽� ������ 0.1�� �Ŀ� ����, �� npc�� �Ľ� Ȯ�ο� �ڷ�ƾ
    {
        yield return new WaitForSeconds(0.1f);

        foreach (KeyValuePair<int, List<string>> dialogueEntry in m_dialogueDic)
        {
            Debug.Log($"ID: {dialogueEntry.Key}");
            foreach (string dialogue in dialogueEntry.Value)
            {
                Debug.Log($"{dialogue}");
            }
        }
    }
}
