using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int maxHp;
    [HideInInspector] public int curHp;
    public int maxMp;
    [HideInInspector] public int curMp;

    private void Start()
    {
        curHp = maxHp;
        curMp = maxMp;
    }
}
