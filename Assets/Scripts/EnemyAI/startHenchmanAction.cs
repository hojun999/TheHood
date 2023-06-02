using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startHenchmanAction : MonoBehaviour
{
    public GameObject gameManager;
    void Update()
    {
        if (gameManager.GetComponent<GameManager>().isEnterFight)
            gameObject.GetComponent<HenchmanAI>().enabled = true;
    }
}
