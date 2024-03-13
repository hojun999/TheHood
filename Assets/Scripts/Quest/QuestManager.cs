using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    [Header("Quest1")]
    public GameObject required_Area_Quest1;     // 퀘스트 1의 숲 맵 특정 지역 탐색, 빈 오브젝트 boxcollider2d 넣어서 oncollisionenter2d로 관리
    public GameObject inProgressQuestImage_Quest1;

    [Header("Quest2")]
    public GameObject required_ItemGroup_Quest2;
    public GameObject inProgressQuestImage_Quest2;
    public int getItemNum_Quest2;


    [Header("Quest3")]
    public GameObject enemyGroup_Quest3;
    public GameObject inProgressQuestImage_Quest3;

    [Header("Quest4")]
    public GameObject enemyGruop_Quest4;
    public GameObject inProgressQuestImage_Quest4;

    [Header("QuestUI")]
    public GameObject questionMark;
    public GameObject exMark;
    public GameObject Direction_Right;
    public GameObject Direction_Up;

    [Header("Direction")]
    public GameObject Direction_BigArea;
    public GameObject Direction_SmallArea;
    public GameObject Direction_Quest3AndQuest4;


    [SerializeField] public int locateAtQuestAreaNum_Quest1;
    [SerializeField] public int eliminateHenchmanNum_Quest3;
    [SerializeField] public int eliminateBossNum_Quest4;
    [SerializeField] public int eliminateHenchmanNum_Quest4;


    Dictionary<int, QuestData> questList;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();

        getItemNum_Quest2 = 3;
    }

    private void GenerateData()     // 퀘스트 목록 생성
    {
        questList.Add(10, new QuestData("점거 지역 탐색", new int[] { 2000}));

        questList.Add(20, new QuestData("흔적 조사", new int[] { 2000}));

        questList.Add(30, new QuestData("부하 사살", new int[] { 2000}));

        questList.Add(40, new QuestData("말머리 패거리 정리", new int[] { 2000}));

        questList.Add(50, new QuestData("엔딩", new int[] { 2000 }));
    }

    public void checkQuest(int id)      // 정해진 npc와 대화를 할 때만 index++
    {
        // 퀘스트 대화가 끝나면  그 퀘스트의 다음 대화 출력
        if (id == questList[questId].npcId[0])
            questActionIndex++;

        // 대화 계속해서 다음 퀘스트로 넘어가지 않게 조절
        if (questActionIndex == 8)
            questActionIndex = 1;

        if (questId == 10)      // 다음 퀘스트로 넘기는 부분은 PlayerController의 oncollisionenter에서 처리
        {
            inProgressQuestImage_Quest1.SetActive(true);        // checkQuest 함수가 대화(spacebar) '직후' 호출되므로 퀘스트UI오브젝트를 여기에 배치함
            required_Area_Quest1.SetActive(true);
            exMark.SetActive(false);
            questionMark.SetActive(true);
            Direction_Right.SetActive(true);
            Direction_Up.SetActive(false);
            Direction_BigArea.SetActive(true);
            Direction_SmallArea.SetActive(true);
        }
        else if (questId == 20)
        {
            inProgressQuestImage_Quest1.SetActive(false);
            inProgressQuestImage_Quest2.SetActive(true);
            required_ItemGroup_Quest2.SetActive(true);
            exMark.SetActive(false);
            questionMark.SetActive(true);
            Direction_Right.SetActive(true);
            Direction_Up.SetActive(false);
        }
        else if (questId == 30)     // 다음 퀘스트로 넘기는 부분 GameManager Update()에서 처리
        {
            inProgressQuestImage_Quest2.SetActive(false);
            inProgressQuestImage_Quest3.SetActive(true);
            enemyGroup_Quest3.SetActive(true);
            exMark.SetActive(false);
            questionMark.SetActive(true);
            Direction_Right.SetActive(true);
            Direction_Up.SetActive(false);
        }
        else if (questId == 40)
        {
            inProgressQuestImage_Quest3.SetActive(false);
            inProgressQuestImage_Quest4.SetActive(true);
            exMark.SetActive(false);
            questionMark.SetActive(true);
            enemyGruop_Quest4.SetActive(true);
            Direction_Quest3AndQuest4.SetActive(true);
            Direction_Right.SetActive(true);
            Direction_Up.SetActive(false);
        }
        else if(questId == 50)
        {
            inProgressQuestImage_Quest4.SetActive(false);
            questionMark.SetActive(false);
            Direction_Right.SetActive(false);
            Direction_Up.SetActive(false);
        }
    }

    public int GetQuestTalkIndex(int id)    // 한 퀘스트 내에서의 다음 대화 출력 (수락 x 상태 대화 > 수락 o 상태 대화)
    {
        return questId + questActionIndex;      // 퀘스트 번호 + 퀘스트 대화 순서
    }

    public void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
}
