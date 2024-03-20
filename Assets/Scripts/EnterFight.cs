using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFight : MonoBehaviour
{
    public GameObject EnterFightUIPanel;
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnterFightUIPanel.SetActive(true);
            gameManager.isUIInteract = true;
            Time.timeScale = 0;
        }
    }


}
