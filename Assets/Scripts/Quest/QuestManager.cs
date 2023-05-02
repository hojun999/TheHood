using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    public GameObject required_Area_Quest1;     // 퀘스트 1의 숲 맵 특정 지역 탐색, 빈 오브젝트 boxcollider2d 넣어서 oncollisionenter2d로 관리
    public GameObject ongoingQuestImage_Quest1;      // 퀘스트 1의 진행 상황 UI
    public GameObject ongoingQuestImage_Clear_Quest1;      // 퀘스트 1의 진행 상황 UI

    public GameObject[] questObject;

    [HideInInspector] public bool isCheckQuestArea;
    [HideInInspector] public bool isGetMalfuncionedGun;
    [HideInInspector] public bool isGetInjector;
    [HideInInspector] public bool isGetClothesWithBlood;
    [HideInInspector] public int eliminateEnemyNum = 8;

    Dictionary<int, QuestData> questList;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    private void GenerateData()     // 퀘스트 목록 생성
    {
        questList.Add(10, new QuestData("점거 지역 탐색", new int[] { 2000 }));

        questList.Add(20, new QuestData("흔적 조사", new int[] { 2000 }));

        questList.Add(30, new QuestData("부하 사살", new int[] { 2000 }));

        questList.Add(40, new QuestData("말머리 패거리 처단", new int[] { 2000 }));

    }

    public string checkQuest(int id)      // 정해진 npc와 대화를 할 때만 index++
    {
        // 퀘스트 대화가 끝나면 그 퀘스트의 다음 대화 출력
        if (id == questList[questId].npcId[0])
            questActionIndex++;

        // 대화 계속해서 다음 퀘스트로 넘어가지 않게 조절
        if (questActionIndex == 5)      
            questActionIndex = 1;

        // 퀘스트에 따라 보여야 할 object 관리.
        ControlObject();

        if (isCheckQuestArea && questId == 10)
            NextQuest();
        if (isGetClothesWithBlood && isGetInjector && isGetMalfuncionedGun && questId == 20)
            NextQuest();
        if (eliminateEnemyNum == 0 && questId == 30)
            NextQuest();

        return questList[questId].questName;
    }

    public int GetQuestTalkIndex(int id)    // 한 퀘스트 내에서의 다음 대화 출력 (수락 x > 수락 o)
    {
        return questId + questActionIndex;      // 퀘스트 번호 + 퀘스트 대화 순서
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    // 퀘스트 이름 반환
    //public string checkQuest()
    //{
    //    return questList[questId].questName;
    //}

    

    void ControlObject()
    {
        switch (questId)
        {
            case 20:    // 두 번째 퀘스트
                {
                    questObject[0].SetActive(true); // 주사기, 찢어진 옷, 고장난 총
                    questObject[1].SetActive(true);
                    questObject[2].SetActive(true);
                }
                break;
            case 30:    // 세 번째 퀘스트
                    questObject[0].SetActive(false); 
                    questObject[1].SetActive(false);
                    questObject[2].SetActive(false); // {} 추가해야하는지 확인
                break;
        }
    }

}
