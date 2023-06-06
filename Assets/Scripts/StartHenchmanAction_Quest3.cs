using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHenchmanAction_Quest3 : MonoBehaviour
{
    void Update()
    {
        if (gameObject.GetComponent<HenchmanAI>().isStartFight) { 
        }
            gameObject.GetComponent<HenchmanAI_Quest3>().enabled = true;
    }
}
