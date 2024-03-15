using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<int, List<string>> m_dialogueDic = new Dictionary<int, List<string>>();       // ID�� ���� ���[] �迭�� ����. ����Ʈ ���� ���¿� ���� ID++, ��� ���� ���¿� ���� index++;

    private int startID;
    [HideInInspector] public int curID;  // client : [����Ʈ Ŭ���� ��] +1, [����Ʈ ���� ��] +10, [����Ʈ �Ϸ� ��] -9
                                                // trador : [Ư�� ���� �̻� Ư�� ������ ���� ���¿��� ��ȣ�ۿ� ��] +1

    private void Start()
    {
        //PrintAllDialogues();
        StartCoroutine(SetFirstID());
        curID = startID;
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

    IEnumerator SetFirstID()        // dicitonary �Ľ� �Ŀ� �Ҵ�ǰ� �ϱ� ����.
    {
        yield return new WaitForSeconds(0.1f);
        startID = m_dialogueDic.Keys.Min();       // npc�� ù ��° ��ȭ �迭�� id ã��

    }
}
