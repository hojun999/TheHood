using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string m_name;
    //public string[] m_contextsArray;
    public Dictionary<int, List<string>> m_dialogueDic = new Dictionary<int, List<string>>();

    [HideInInspector] public int m_dialogueID;

    private void Start()
    {
        //Debug.Log(m_name + "의 대화데이터 : " );
        
            Debug.Log(m_dialogueDic[1]);
    }

}
