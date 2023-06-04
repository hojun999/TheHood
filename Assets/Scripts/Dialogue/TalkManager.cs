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
            "우선 정보가 맞는지 숲에 가서 거점부터 확인하고 보고해." });
        talkData.Add(11 + 2000, new string[] { "꾸물거리지 말고 바로 출발해." });

        //2번 퀘스트 : 흔적 조사
        talkData.Add(20 + 2000, new string[] { "정말 거점을 만들었나보군.",
            "다음 임무다. 거점의 위치를 알았으니 흔적을 찾아와.",
            "그놈들이 사용한 어떤것이라도 상관없어. 물증이 생기면 공격한다.",
            "혹시 놈들을 마주치면 조용히 처리하고. 흔적을 모두 찾으면 보고해." });
        talkData.Add(21 + 2000, new string[] { "아직 못찾았나?" });

        //3번 퀘스트 : 부하 사살
        talkData.Add(30 + 2000, new string[] { "약물까지 사용하다니 놈들의 몰골이 궁금하구만.",
            "물건의 상태를 보니 놈들도 좋은 상황은 아닌가 본데,",
            "마침 이번에 들어온 정보에 의하면 말 머리놈이 자리를 비운 모양이야.",
            "가서 부하놈들을 상대해주고 오라고."});
        talkData.Add(31 + 2000, new string[] { "멍청한 놈들." });

        //4번 퀘스트 : 말머리 패거리 처단
        talkData.Add(40 + 2000, new string[] { "좋은 결과다. 놈들의 머릿수가 많이 줄었어.",
            "정보에 의하면 말머리도 좋은 장비는 없다고 한다.",
            "때가 됐다. 가서 패거리를 다 죽이고 와.",});
        talkData.Add(41 + 2000, new string[] { "이번 일만 성공하면 숲이 당분간 조용하겠군." });

        //엔딩 대화
        talkData.Add(50 + 2000, new string[] { "전쟁의 떨거지들이 다 죽었군.",
            "당분간 마을 사람들이 힘들지는 않겠어.",
            "이번 임무로 평화에 조금 더 가까워졌다.",
            "수고했다. 후드. 다음 임무까지 쉬도록 해."});

        //Trade Talk
        //Weapon
        talkData.Add(1000, new string[] { "요즘 숲에서 질 좋은 광석이 굴러다닌다던데,",
            "3개만 가져오면 방어구를 강화해주지.",
            "너의 체력이 올라갈지도?"});

        //posion
        talkData.Add(3000, new string[] { "이번 임무는 숲인가요, 후드?",
            "숲에서 약초를 주워오면 포션을 만들어줄게요.",
            "체력 포션은 빨간약초 2개, 에너지 포션은 파란 약초 2개에요."});

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
