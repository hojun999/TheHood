using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterWoods : MonoBehaviour
{
    public GameObject EnterUIPanel;
    public GameObject soundManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            EnterUIPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        soundManager.GetComponent<SoundManager>().EnterWoods();
    }
}
