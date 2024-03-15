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
    public Dictionary<int, List<string>> m_dialogueDic = new Dictionary<int, List<string>>();       // ID에 따른 대사[] 배열을 가짐. 퀘스트 진행 상태에 따라 ID++, 대사 진행 상태에 따라 index++;

    private int startID;
    [HideInInspector] public int curID;  // client : [퀘스트 클리어 시] +1, [퀘스트 시작 시] +10, [퀘스트 완료 시] -9
                                                // trador : [특정 개수 이상 특정 아이템 보유 상태에서 상호작용 시] +1

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

    IEnumerator SetFirstID()        // dicitonary 파싱 후에 할당되게 하기 위함.
    {
        yield return new WaitForSeconds(0.1f);
        startID = m_dialogueDic.Keys.Min();       // npc별 첫 번째 대화 배열의 id 찾기

    }
}
