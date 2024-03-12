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

    #region
    private void GenerateData()         // �⺻/����Ʈ ��� �߰�
    {
        // Talk Data
        // NPC Client : 1000(Weapon), 2000(Quest), 3000(Posion) 
        talkData.Add(2000, new string[] { "�� �� ����?" });

        //Quest Talk
        //1�� ����Ʈ : ���� ���� Ž��
        talkData.Add(10 + 2000, new string[] { "�ӹ���. �ĵ�.",
            "�� ���Ӹ��� �� �� ���� ������ �Դ�.",
            "�� ���� �аŸ��� ���� ������ ��� �ֺ� ������ ��Ż�ϴ� ����̾�.",
            "�Ӹ����� ������, ������ �����ϰ� �־ �̹� ���� ��� �� ���� �׾��ٰ� �Ѵ�.",
            "�켱 ������ �´��� ���� ���� �������� Ȯ���ϰ� ������." });
        talkData.Add(11 + 2000, new string[] { "�ٹ��Ÿ��� ���� �ٷ� �����." });

        //2�� ����Ʈ : ���� ����
        talkData.Add(20 + 2000, new string[] { "���� ������ �����������.",
            "���� �ӹ���. ������ ��ġ�� �˾����� ������ ã�ƿ�.",
            "����� ����� ����̶� �������. ������ ����� �ٷ� �����Ѵ�.",
            "���� ����� ����ġ���� ����. ������ ��� ã���� ������." });
        talkData.Add(21 + 2000, new string[] { "���� ��ã�ҳ�?" });

        //3�� ����Ʈ : ���� ���
        talkData.Add(30 + 2000, new string[] { "�๰���� ����ϴٴ� ����� ������ �ñ��ϱ���.",
            "������ ���¸� ���� ��鵵 ���� ��Ȳ�� �ƴѰ� ����,",
            "��ħ �̹��� ���� ������ ���ϸ� �� �Ӹ����� �ڸ��� ��� ����̾�.",
            "�ڸ��� ��� ƴ�� Ÿ�� ���� ���� ���ƴٴϴ� ��û�� ���ϳ���� �Ӹ����� ���̰� ��."});

        talkData.Add(31 + 2000, new string[] { "�� �����." });

        //4�� ����Ʈ : ���Ӹ� �аŸ� ó��
        talkData.Add(40 + 2000, new string[] { "���� �����. ����� �Ӹ����� ���� �پ���.",
            "�̹��� ���� ������ ���ϸ� ���Ӹ��� ���� ���� ���ٰ� �Ѵ�.",
            "���� �ƴ�. ���� �аŸ��� �� ���̰� ��.",});
        talkData.Add(41 + 2000, new string[] { "�̹� �ӹ��� �����ϸ� ���� ��а� �����ϰھ�." });

        //���� ��ȭ
        talkData.Add(50 + 2000, new string[] { "������ ���������� �� �׾���.",
            "��а� ���� ������� �������� �ʰھ�.",
            "�̹� �ӹ��� ��ȭ�� ���� �� ���������.",
            "�����ߴ�. �ĵ�. ���� �ӹ����� ������ ��."});

        //Trade Talk
        //Weapon
        talkData.Add(1000, new string[] { "���� ������ �� ���� ������ �����ٴѴٴ���,",
            "3���� �������� ���� ��ȭ������.",
            "���� ü���� �ö�����?"});

        talkData.Add(1 + 1000, new string[] { "��ȭ������ Ȯ���غ�." });

        //posion
        talkData.Add(3000, new string[] { "�̹� �ӹ��� ���ΰ���, �ĵ�?",
            "������ ���ʸ� �ֿ����� ������ ������ٰԿ�.",
            "ü�� ������ �������� 2��, ������ ������ �Ķ� ���� 2���� �ʿ��ؿ�.",});


        talkData.Add(1 + 3000, new string[] { "���� �����̿���." });

        talkData.Add(2 + 3000, new string[] { "���� �߰��ϸ� �� �����Ϳ�!" });
    }
    #endregion

    public string GetTalk(int id, int talkIndex)
    {
        //��ȭ ����ó��
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
