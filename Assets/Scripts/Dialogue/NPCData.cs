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

    IEnumerator coPrintAllDialogues()       // 정상적으로 파싱이 되지만, 메서드 호출이 겹쳐서 콘솔창에 안보이는 이슈 때문에 0.1초 후에 실행, 각 npc별 파싱 확인용 코루틴
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
