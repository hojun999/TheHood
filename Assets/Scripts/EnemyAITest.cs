using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITest : MonoBehaviour
{
    public enum currentState
    {
        wait,
        move,
        attack,
        die
    }

    public currentState curState = currentState.wait;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
