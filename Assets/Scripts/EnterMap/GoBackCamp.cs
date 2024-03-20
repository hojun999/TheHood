using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackCamp : MonoBehaviour
{
    public GameObject EnterUIPanel;
    public GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            gameManager.isUIInteract = true;
            EnterUIPanel.SetActive(true);
        }
    }

}
