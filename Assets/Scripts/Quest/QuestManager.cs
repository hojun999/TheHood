using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    public GameObject[] questObject;

    Dictionary<int, QuestData> questList;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();

    }

    private void GenerateData()     // 퀘스트 목록 생성
    {
        questList.Add(10, new QuestData("점거 지역 탐색", new int[] { 1000 }));

        questList.Add(20, new QuestData("흔적 조사", new int[] { 1000 }));

        questList.Add(30, new QuestData("부하들 사살", new int[] { 1000 }));

        questList.Add(40, new QuestData("말머리 패거리 사살", new int[] { 1000 }));



    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string checkQuest(int id)      // 정해진 npc와 대화를 할 때만 index++
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;         // 퀘스트 대화가 끝나면 다음 대화로 순서 변경(가이드 영상에서는 1번npc > 2번npc 이므로 이 함수와 밑의 if문은 서로 다른 npc의 대화를 호출함. 나는 같은 npc에서 대화를 호출하므로 이에 대해서 다시 작성해야됨.

        // Control Quest Object
        ControlObject();            // 퀘스트에 따라 보여야 할 object 관리. 밑의 if문 두개 수정하고 확인하기.

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }

    public string checkQuest()
    {
        // 퀘스트 이름 반환
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 1)
                    questObject[0].SetActive(true);
                break;
            case 20:
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }
    }

}
