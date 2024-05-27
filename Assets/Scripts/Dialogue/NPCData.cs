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
        trader
    }
    public m_type NPCType;
 
    public Dictionary<int, List<string>> m_dialogueDic = new Dictionary<int, List<string>>();       // ���

    private int startID;                    // ���� ��ȭ ID ���� ����
    public int curID;     // ���� ���� ��ȭ ID ���� ����

    private void Start()
    {
        //PrintAllDialogues();
        StartCoroutine(SetFirstID());
    }
    public void PrintAllDialogues()
    {
        StartCoroutine(coPrintAllDialogues());
    }

    IEnumerator coPrintAllDialogues()       // ���������� �Ľ��� ������, �ܼ�â�� �Ⱥ��̴� �̽� ������ 0.1�� �Ŀ� ����, �� npc�� �Ľ� Ȯ�ο� �ڷ�ƾ
    {
        yield return new WaitForSeconds(0.1f);

        foreach (var pair in m_dialogueDic)
        {
            Debug.Log($"Key: {pair.Key}");
            Debug.Log("Values:");
            foreach (var value in pair.Value)
            {
                Debug.Log(value);
            }
            Debug.Log("");
        }

    }

    IEnumerator SetFirstID()        // dicitonary �Ľ� �Ŀ� �Ҵ�ǰ� �ϱ� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(0.1f);
        startID = m_dialogueDic.Keys.Min();       // npc�� ù ��° ��ȭ �迭�� id ã��
        curID = startID;
    }

    public void SetNextID()
    {
        curID++;
    }
}
