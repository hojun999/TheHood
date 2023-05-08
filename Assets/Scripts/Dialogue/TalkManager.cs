using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;


    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData()         // 기본/퀘스트 대사 추가
    {
        // Talk Data
        // NPC Client : 1000(Weapon), 2000(Quest), 3000(Posion) 
        talkData.Add(2000, new string[] { "할 일 없나?" });

        //Quest Talk
        //1번 퀘스트 : 점거 지역 탐색
        talkData.Add(10 + 2000, new string[] { "임무다. 후드.",
            "왠 말머리를 쓴 놈에 대한 제보가 왔다.",
            "그 놈의 패거리가 숲에 거점을 잡고 주변 마을을 약탈하는 모양이야.",
            "총으로 무장하고 있다는데, 이미 마을 사람 몇 명은 죽었다고 한다.",
            "우선 제보가 맞는지 숲에 가서 거점부터 확인하고 보고해." });
        talkData.Add(11 + 2000, new string[] { "꾸물거리지 말고 바로 출발해." });

        //2번 퀘스트 : 흔적 조사
        talkData.Add(20 + 2000, new string[] { "정말 거점을 만들었나보군.",
            "다음 임무다. 거점의 위치를 알았으니 흔적을 찾아와. 그놈들이 사용한 어떤것이라도 상관없어.",
            "그놈들이 지내고 있다는 물증이 생기면 바로 공격한다.",
            "혹시 모르니 마주치지 않게 조심하고. 흔적을 모두 찾으면 보고해." });
        talkData.Add(21 + 2000, new string[] { "아직 못찾았나?" });

        //3번 퀘스트 : 부하 사살
        talkData.Add(30 + 2000, new string[] { "흔적의 상태가 처참한걸 보니 놈들도 좋은 상황은 아닌가보군." +
            " 약물까지 꼽다니 갈 때까지 갔어.",
            "이번에 들어온 정보에 의하면 말 머리놈이 자리를 비운 모양이야.",
            "가서 부하놈들을 상대해주고 오라고."});
        talkData.Add(31 + 2000, new string[] { "쓰레기같은 놈들." });

        //4번 퀘스트 : 말머리 패거리 처단
        talkData.Add(40 + 2000, new string[] { });

    }

    public string GetTalk(int id, int talkIndex)
    {
        //대화 예외처리
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex);
            else
                return GetTalk(id - id % 10 + 1, talkIndex);
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

}
