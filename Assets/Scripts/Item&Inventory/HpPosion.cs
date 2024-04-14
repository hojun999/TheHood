using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item/PosionItem/HpPosion")]
public class HpPosion : ItemData, IUsable
{
    private PlayerStat _playerStat;
    [SerializeField] private int healingPower = 10;

    public void UseItem()
    {
        _playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        _playerStat.curHp += healingPower;
    }
}
