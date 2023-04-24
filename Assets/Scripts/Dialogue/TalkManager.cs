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
        //1번 퀘스트 : 보스의 거점 확인
        talkData.Add(10 + 2000, new string[] { "임무다. 후드.", "왠 말머리를 쓴 놈에 대한 제보가 왔다.", "그 놈의 패거리가 숲에 거점을 잡고 주변 마을을 약탈하는 모양이야.", "총으로 무장하고 있다는데, 이미 마을 사람 몇 명은 죽었다고 한다.", "우선 제보가 맞는지 숲에 가서 거점부터 확인하고 보고해." });
        talkData.Add(11 + 2000, new string[] { "꾸물거리지 말고 바로 출발해." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        //대화 예외처리
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex);
            else
                return GetTalk(id - id % 10, talkIndex);
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

}
