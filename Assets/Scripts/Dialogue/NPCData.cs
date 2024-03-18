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
    public Dictionary<int, List<string>> m_dialogueDic = new Dictionary<int, List<string>>();       // ID에 따른 대사[] 배열. 퀘스트 진행 상태에 따라 ID++, 대사 진행 상태에 따라 index++;

    private int startID;                    // 시작 대화 ID 저장 변수
    [HideInInspector] public int curID;     // 진행 중인 대화 ID 저장 변수

    private void Start()
    {
        //PrintAllDialogues();
        StartCoroutine(SetFirstID());
    }
    public void PrintAllDialogues()
    {
        StartCoroutine(coPrintAllDialogues());
    }

    IEnumerator coPrintAllDialogues()       // 정상적으로 파싱이 되지만, 콘솔창에 안보이는 이슈 때문에 0.1초 후에 실행, 각 npc별 파싱 확인용 코루틴
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

    IEnumerator SetFirstID()        // dicitonary 파싱 후에 할당되게 하기 위한 코루틴
    {
        yield return new WaitForSeconds(0.1f);
        startID = m_dialogueDic.Keys.Min();       // npc별 첫 번째 대화 배열의 id 찾기
        curID = startID;
    }
}
