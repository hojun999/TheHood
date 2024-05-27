using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI_EnterFight : MonoBehaviour
{
    GameManager gameManager;
    public GameObject enter_fight_UI;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enter_fight_UI.SetActive(true);
            gameManager.isUIInteract = true;
            Time.timeScale = 0;
        }
    }
}
