using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{

    public void HealingPlayer(int healingPower)
    {
        Debug.Log("Player Hp : + " + healingPower);
    }

    public void BuffSpeed(float SpeedUpPower)
    {
        Debug.Log("Player Speed : + " + SpeedUpPower);
    }
}
