using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBossAction : MonoBehaviour
{
    public GameObject gameManager;

    void Update()
    {
        if (gameManager.GetComponent<GameManager>().isEnterFight)
            gameObject.GetComponent<BossAI>().enabled = true;
    }
}
